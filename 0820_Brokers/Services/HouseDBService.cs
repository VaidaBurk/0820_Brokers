using _0820_Brokers.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace _0820_Brokers.Services
{
    public class HouseDBService
    {
        private readonly SqlConnection _connection;
        public HouseDBService(SqlConnection sqlConnection)
        {
            _connection = sqlConnection;
        }
        public List<HouseModel> GetData()
        {
            List<HouseModel> houses = new();
            _connection.Open();
            SqlCommand command = new($@"SELECT h.*, CONCAT(h.Street, ' ', h.HouseNo, '- ', h.FlatNo), c.Name
                                        FROM Houses h
                                        JOIN Companies c
                                        ON h.CompanyId = c.CompanyId;", _connection);
            using var Reader = command.ExecuteReader();
            while (Reader.Read())
            {
                houses.Add(new()
                {
                    FlatId = Reader.GetInt32(0),
                    City = Reader.GetString(1),
                    Street = Reader.GetString(2),
                    HouseNo = Reader.GetString(3),
                    FlatNo = Reader.GetString(4),
                    FlatFloor = Reader.GetInt32(5),
                    BuildingFloors = Reader.GetInt32(6),
                    Area = Reader.GetDecimal(7),
                    CompanyId = Reader.GetInt32(9),
                    FullAddress = Reader.GetString(10),
                    CompanyName = Reader.GetString(11)
                });
            }
            _connection.Close();
            return houses;
        }

        public void SaveToDatabase(HouseModel house)
        {
            _connection.Open();
            SqlCommand command = new($@"INSERT INTO Houses (City, Street, HouseNo, FlatNo, FlatFloor, BuildingFloors, Area, CompanyId)
                                        VALUES ('{house.City}', '{house.Street}', '{house.HouseNo}', '{house.FlatNo}', '{house.FlatFloor}', '{house.BuildingFloors}', '{house.Area}', '{house.CompanyId}');",
                                        _connection);
            command.ExecuteNonQuery();
            _connection.Close();
        }

    }
}
