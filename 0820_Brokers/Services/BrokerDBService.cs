using _0820_Brokers.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace _0820_Brokers.Services
{
    public class BrokerDBService
    {
        private readonly SqlConnection _connection;
        public BrokerDBService(SqlConnection connection)
        {
            _connection = connection;
        }
        public List<BrokerModel> GetData()
        {
            List<BrokerModel> Brokers = new();
            _connection.Open();
            SqlCommand command = new($@"SELECT BrokerId, Name, Surname, CONCAT(Name, ' ', Surname) AS FullName
                                        FROM Brokers;", _connection);
            using var Reader = command.ExecuteReader();
            while (Reader.Read())
            {
                Brokers.Add(new()
                {
                    BrokerId = Reader.GetInt32(0),
                    Name = Reader.GetString(1),
                    Surname = Reader.GetString(2),
                    FullName = Reader.GetString(3)
                });
            }
            _connection.Close();
            return Brokers;
        }

        public void SaveToDatabase(BrokerModel broker)
        {
            _connection.Open();
            SqlCommand command = new($@"INSERT INTO Brokers (Name, Surname)
                                        VALUES ('{broker.Name}', '{broker.Surname}');", _connection);
            command.ExecuteNonQuery();
            _connection.Close();
        }
        public List<BrokerModel> GetNotCompanyBrokers(int companyId)
        {
            List<BrokerModel> notCompanyBrokers = new();
            _connection.Open();
            SqlCommand command = new($@"SELECT b.BrokerId, b.Name, b.Surname, CONCAT(Name, ' ', Surname) AS FullName, cs.CompanyId
                                        FROM brokers b
                                        LEFT JOIN CompanyBroker cs
                                        ON b.BrokerId = cs.BrokerId
                                        WHERE cs.CompanyId != {companyId} OR cs.CompanyId IS NULL;", _connection);
            using var Reader = command.ExecuteReader();
            while (Reader.Read())
            {
                notCompanyBrokers.Add(new()
                {
                    BrokerId = Reader.GetInt32(0),
                    Name = Reader.GetString(1),
                    Surname = Reader.GetString(2),
                    FullName = Reader.GetString(3)
                });
            }
            _connection.Close();
            return notCompanyBrokers;
        }
        

        public List<HouseModel> HousesBrokerCanChoose(int brokerId)
        {
            List<HouseModel> houses = new();
            _connection.Open();
            SqlCommand command = new($@"SELECT *, CONCAT(Street, ' ', HouseNo, '- ', FlatNo)
                                        FROM Houses h
                                        LEFT JOIN CompanyBroker cb
                                        ON h.CompanyId = cb.CompanyId
                                        WHERE h.BrokerId IS NULL AND cb.BrokerId = {brokerId};", _connection);
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
                    FullAddress = Reader.GetString(12)
                });
            }
            _connection.Close();
            return houses;
        }
        public void AssignAppartmentToBroker(int houseId, int brokerId)
        {
            _connection.Open();
            SqlCommand command = new($@"UPDATE Houses
                                        SET BrokerId = {brokerId}
                                        WHERE FlatId = {houseId};", _connection);
            command.ExecuteNonQuery();
            _connection.Close();
        }
        //public List<int> GetBrokerCompanies (int brokerId)
        //{
        //    List<int> companiesId = new();
        //    _connection.Open();
        //    SqlCommand command = new($@"SELECT CompanyId
        //                                FROM CompanyBroker
        //                                WHERE BrokerId = {brokerId}", _connection);
        //    using var Reader = command.ExecuteReader();
        //    while (Reader.Read())
        //    {
        //        companiesId.Add(Reader.GetInt32(0));
        //    }
        //    _connection.Close();
        //    return companiesId;
        //}
    }

}
