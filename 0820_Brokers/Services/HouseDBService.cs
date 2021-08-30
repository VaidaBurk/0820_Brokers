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
            SqlCommand command = new($@"SELECT h.*, CONCAT(h.Street, ' ', h.HouseNo, '- ', h.FlatNo), c.Name, CONCAT(b.Name, ' ', b.Surname)
                                        FROM Houses h
                                        LEFT JOIN Companies c
                                        ON h.CompanyId = c.CompanyId
                                        LEFT JOIN Brokers b
                                        ON h.BrokerId = b.BrokerId;", _connection);
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
                    CompanyName = Reader.GetString(11),
                    Broker = Reader.GetString(12)
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
       

        public void RemoveAppartmentFromBroker(int houseId)
        {
            _connection.Open();
            SqlCommand command = new($@"UPDATE Houses
                                        SET BrokerId = NULL 
                                        WHERE FlatId = {houseId};", _connection);
            command.ExecuteNonQuery();
            _connection.Close();
        }

        public void DeleteAppartment(int houseId)
        {
            _connection.Open();
            SqlCommand command = new($@"DELETE FROM Houses
                                        WHERE FlatId = {houseId};", _connection);
            command.ExecuteNonQuery();
            _connection.Close();
        }
        public List<HouseModel> GetHouseData(int houseId)
        {
            List<HouseModel> houses = new();
            _connection.Open();
            SqlCommand command = new($@"SELECT h.*, CONCAT(h.Street, ' ', h.HouseNo, '- ', h.FlatNo), c.Name, CONCAT(b.Name, ' ', b.Surname)
                                        FROM Houses h
                                        LEFT JOIN Companies c
                                        ON h.CompanyId = c.CompanyId
                                        LEFT JOIN Brokers b
                                        ON h.BrokerId = b.BrokerId
                                        WHERE h.FlatId = {houseId};", _connection);
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
                    CompanyName = Reader.GetString(11),
                    Broker = Reader.GetString(12)
                });
            }
            _connection.Close();
            return houses;
        }
        public void EditInDatabase(HouseModel house)
        {
            _connection.Open();
            SqlCommand command = new($@"UPDATE Houses 
                                        SET City = '{house.City}', 
	                                        Street = '{house.Street}', 
	                                        HouseNo = '{house.HouseNo}',
	                                        FlatNo = '{house.FlatNo}',
	                                        FlatFloor = '{house.FlatFloor}',
	                                        BuildingFloors = '{house.BuildingFloors}', 
	                                        Area = '{house.Area}'
                                        WHERE FlatId = {house.FlatId};", _connection);
            command.ExecuteNonQuery();
            _connection.Close();
        }
       
        public List<string> GetAllCities()
        {
            List<string> cities = new();
            _connection.Open();
            SqlCommand command = new($@"SELECT DISTINCT City
                                        FROM Houses", _connection);
            using var Reader = command.ExecuteReader();
            while(Reader.Read())
            {
                cities.Add(Reader.GetString(0));
            }
            _connection.Close();
            return cities;
        }
        public List<HouseModel> GetFilteredByCity(string cityName)
        {
            List<HouseModel> houses = new();
            _connection.Open();
            SqlCommand command = new($@"SELECT h.*, CONCAT(h.Street, ' ', h.HouseNo, '- ', h.FlatNo), c.Name, CONCAT(b.Name, ' ', b.Surname)
                                        FROM Houses h
                                        LEFT JOIN Companies c
                                        ON h.CompanyId = c.CompanyId
                                        LEFT JOIN Brokers b
                                        ON h.BrokerId = b.BrokerId
                                        WHERE h.City = '{cityName}';", _connection);
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
                    CompanyName = Reader.GetString(11),
                    Broker = Reader.GetString(12)
                });
            }
            _connection.Close();
            return houses;
        }
        public List<HouseModel> GetFilteredByCompany(int companyId)
        {
            List<HouseModel> houses = new();
            _connection.Open();
            SqlCommand command = new($@"SELECT h.*, CONCAT(h.Street, ' ', h.HouseNo, '- ', h.FlatNo), c.Name, CONCAT(b.Name, ' ', b.Surname)
                                        FROM Houses h
                                        LEFT JOIN Companies c
                                        ON h.CompanyId = c.CompanyId
                                        LEFT JOIN Brokers b
                                        ON h.BrokerId = b.BrokerId
                                        WHERE h.CompanyId = {companyId};", _connection);
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
                    CompanyName = Reader.GetString(11),
                    Broker = Reader.GetString(12)
                });
            }
            _connection.Close();
            return houses;
        }
        public List<HouseModel> GetFilteredByBroker(int brokerId)
        {
            List<HouseModel> houses = new();
            _connection.Open();
            SqlCommand command = new($@"SELECT h.*, CONCAT(h.Street, ' ', h.HouseNo, '- ', h.FlatNo), c.Name, CONCAT(b.Name, ' ', b.Surname)
                                        FROM Houses h
                                        LEFT JOIN Companies c
                                        ON h.CompanyId = c.CompanyId
                                        LEFT JOIN Brokers b
                                        ON h.BrokerId = b.BrokerId
                                        WHERE h.BrokerId = {brokerId};", _connection);
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
                    CompanyName = Reader.GetString(11),
                    Broker = Reader.GetString(12)
                });
            }
            _connection.Close();
            return houses;
        }
    }
}
