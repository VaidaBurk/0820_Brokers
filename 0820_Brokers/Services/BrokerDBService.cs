﻿using _0820_Brokers.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

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
            SqlCommand command = new($@"SELECT b.*, CONCAT(b.Name, ' ', b.Surname) AS FullName, STRING_AGG(c.Name, ', ') AS Companies
                                        FROM Brokers b
                                        JOIN CompanyBroker cb
                                        ON b.BrokerId = cb.BrokerId
                                        JOIN Companies c
                                        ON cb.CompanyId = c.CompanyId
                                        GROUP BY b.BrokerId, b.Name, b.Surname;", _connection);
            using var Reader = command.ExecuteReader();
            while (Reader.Read())
            {
                Brokers.Add(new()
                {
                    BrokerId = Reader.GetInt32(0),
                    Name = Reader.GetString(1),
                    Surname = Reader.GetString(2),
                    FullName = Reader.GetString(3),
                    Company = Reader.GetString(4)
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
            SqlCommand command = new($@"SELECT b.BrokerId, b.Name, b.Surname, CONCAT(Name, ' ', Surname) AS FullName
                                        FROM brokers b
                                        LEFT JOIN CompanyBroker cs
                                        ON b.BrokerId = cs.BrokerId
                                        WHERE cs.CompanyId != {companyId} OR cs.CompanyId IS NULL
                                        GROUP BY b.BrokerId, b.Name, b.Surname
                                        ORDER BY b.Surname;", _connection);
            using var Reader = command.ExecuteReader();
            while (Reader.Read())
            {
                notCompanyBrokers.Add(new()
                {
                    BrokerId = Reader.GetInt32(0),
                    Name = Reader.GetString(1),
                    Surname = Reader.GetString(2),
                    FullName = Reader.GetString(3),
                });
            }
            _connection.Close();
            return notCompanyBrokers;
        }
        

        public List<HouseModel> HousesBrokerCanChoose(int brokerId)
        {
            List<HouseModel> houses = new();
            _connection.Open();
            SqlCommand command = new($@"SELECT h.*, CONCAT(h.Street, ' ', h.HouseNo, '- ', h.FlatNo), c.Name
                                        FROM Houses h
                                        LEFT JOIN CompanyBroker cb
                                        ON h.CompanyId = cb.CompanyId
                                        JOIN Companies c
                                        ON cb.CompanyId = c.CompanyId
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
                    FullAddress = Reader.GetString(10),
                    CompanyName = Reader.GetString(11)
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
        public string GetBrokerName(int brokerId)
        {
            _connection.Open();
            SqlCommand command = new($@"SELECT CONCAT(Name, ' ', Surname) AS FullName
                                        FROM Brokers
                                        WHERE BrokerId = {brokerId}", _connection);
            string fullName = command.ExecuteScalar().ToString();
            _connection.Close();
            return fullName;
        }

        public BrokerModel GetBrokerData(int brokerId)
        {
            BrokerModel broker = new();
            _connection.Open();
            SqlCommand command = new($@"SELECT *
                                        FROM Brokers
                                        WHERE BrokerId = {brokerId};", _connection);
            using (var reader = command.ExecuteReader())
            {
                reader.Read();
                broker.BrokerId = reader.GetInt32(0);
                broker.Name = reader.GetString(1);
                broker.Surname = reader.GetString(2);
            }
            _connection.Close();
            return broker;
        }
        public void EditBrokerInDatabase(BrokerModel broker)
        {
            _connection.Open();
            SqlCommand command = new($@"UPDATE Brokers
                                        SET Name = '{broker.Name}', Surname = '{broker.Surname}' 
                                        WHERE BrokerId = {broker.BrokerId};", _connection);
            command.ExecuteNonQuery();
            _connection.Close();
        }
        public List<HouseModel> GetBrokerAppartmentsFromDB(int brokerId)
        {
            List<HouseModel> brokerAppartments = new();
            _connection.Open();
            SqlCommand command = new($@"SELECT h.*, CONCAT(h.Street, ' ', h.HouseNo, '- ', h.FlatNo) AS FullAddress, c.Name, CONCAT(b.Name, ' ', b.Surname) AS BrokerName
                                        FROM Houses h
                                        JOIN Brokers b
                                        ON h.BrokerId = b.BrokerId
                                        JOIN Companies c
                                        ON h.CompanyId = c.CompanyId
                                        WHERE h.BrokerId = {brokerId};", _connection);
            using var Reader = command.ExecuteReader();
            while (Reader.Read())
            {
                brokerAppartments.Add(new()
                {
                    FlatId = Reader.GetInt32(0),
                    City = Reader.GetString(1),
                    Street = Reader.GetString(2),
                    HouseNo = Reader.GetString(3),
                    FlatNo = Reader.GetString(4),
                    FlatFloor = Reader.GetInt32(5),
                    BuildingFloors = Reader.GetInt32(6),
                    Area = Reader.GetDecimal(7),
                    BrokerId = Reader.GetInt32(8),
                    CompanyId = Reader.GetInt32(9),
                    FullAddress = Reader.GetString(10),
                    CompanyName = Reader.GetString(11),
                    Broker = Reader.GetString(12)
                });
            }
            _connection.Close();
            return brokerAppartments;
        }
        public void RemoveAppartmentFromBroker(int houseId)
        {
            _connection.Open();
            SqlCommand command = new($@"UPDATE Houses
                                        SET BrokerId = NULL 
                                        WHERE FlatId = {houseId}", _connection);
            command.ExecuteNonQuery();
            _connection.Close();
        }
    }

}
