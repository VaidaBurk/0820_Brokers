using _0820_Brokers.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace _0820_Brokers.Services
{
    public class CompanyDBService
    {
        private readonly SqlConnection _connection;
        public CompanyDBService(SqlConnection sqlConnection)
        {
            _connection = sqlConnection;
        }
        public List<CompanyModel> GetData()
        {
            List<CompanyModel> companies = new();
            _connection.Open();
            SqlCommand command = new($@"SELECT CompanyId, Name, CONCAT(Street, ' ', HouseNo, ', ', City) AS StreetAddress 
                                        FROM Companies;", _connection);
            using var Reader = command.ExecuteReader();
            while (Reader.Read()){
                companies.Add(new()
                {
                    CompanyId = Reader.GetInt32(0),
                    Name = Reader.GetString(1),
                    StreetAddress = Reader.GetString(2)
                });
            };
            _connection.Close();
            return companies;
        }
        public void SaveToDatabase(CompanyCreateModel newCompany)
        {
            _connection.Open();
            SqlCommand command = new($@"INSERT INTO Companies (Name, City, Street, HouseNo)
                                        VALUES ('{newCompany.Company.Name}', '{newCompany.Company.City}', '{newCompany.Company.Street}', '{newCompany.Company.HouseNo}');",
                                        _connection);
            command.ExecuteNonQuery();
            _connection.Close();

            int id = GetCompanyId(newCompany);

            foreach (int entry in newCompany.BrokerIds)
            {
                _connection.Open();

                command = new($@"INSERT INTO CompanyBroker (CompanyId, BrokerId) 
                                VALUES ('{id}', '{entry}');", _connection);

                command.ExecuteNonQuery();
                _connection.Close();
            }
        }

        public void SaveBrokerToCompany(CompanyCreateModel company)
        {
            foreach (int entry in company.BrokerIds)
            {
                _connection.Open();

                SqlCommand command = new($@"INSERT INTO CompanyBroker (CompanyId, BrokerId) 
                                VALUES ('{company.Company.CompanyId}', '{entry}');", _connection);
                command.ExecuteNonQuery();
                _connection.Close();
            }
        }

        public int GetCompanyId(CompanyCreateModel newCompany)
        {
            _connection.Open();
            SqlCommand command = new($@"SELECT MAX(CompanyId) FROM Companies 
                                        WHERE Name = '{newCompany.Company.Name}' 
                                        AND City = '{newCompany.Company.City}'", _connection);
            int id = (Int32)command.ExecuteScalar();
            _connection.Close();
            return id;
        }

        public List<CompanyModel> GetCompanyById(int companyId)
        {
            List<CompanyModel> companies = new();
            _connection.Open();
            SqlCommand command = new($@"SELECT Name, CONCAT(Street, ' ', HouseNo, ', ', City) AS StreetAddress 
                                        FROM Companies
                                        WHERE CompanyId = {companyId};", _connection);
            using var Reader = command.ExecuteReader();
            while (Reader.Read())
            {
                companies.Add(new()
                {
                    Name = Reader.GetString(0),
                    StreetAddress = Reader.GetString(1)
                });
            };
            _connection.Close();
            return companies;
        }
        public List<BrokerModel> GetCompanyBrokers(int companyId)
        {
            List<BrokerModel> companyBrokers = new();
            _connection.Open();
            SqlCommand command = new($@"SELECT b.BrokerId, b.Name, b.Surname, CONCAT(Name, ' ', Surname) AS FullName
                                        FROM brokers b
                                        LEFT JOIN CompanyBroker cs
                                        ON b.BrokerId = cs.BrokerId
                                        WHERE cs.CompanyId = {companyId}
                                        GROUP BY b.BrokerId, b.Name, b.Surname;", _connection);
            using var Reader = command.ExecuteReader();
            while (Reader.Read())
            {
                companyBrokers.Add(new()
                {
                    BrokerId = Reader.GetInt32(0),
                    Name = Reader.GetString(1),
                    Surname = Reader.GetString(2),
                    FullName = Reader.GetString(3),
                });
            }
            _connection.Close();
            return companyBrokers;
        }
        public string GetCompanyName(int companyId)
        {
            _connection.Open();
            SqlCommand command = new($@"SELECT Name
                                        FROM Companies
                                        WHERE CompanyId = {companyId}", _connection);
            string companyName = command.ExecuteScalar().ToString();
            _connection.Close();
            return companyName;
        }
        public void RemoveBrokerFromCompany(int brokerId, int companyId)
        {
            _connection.Open();
            SqlCommand command = new($@"DELETE FROM CompanyBroker
                                        WHERE CompanyId = {companyId} AND BrokerId = {brokerId};", _connection);
            command.ExecuteNonQuery();
            _connection.Close();
        }
        public List<HouseModel> GetCompanyAppartmentsFromDB(int companyId)
        {
            List<HouseModel> companyAppartments = new();
            _connection.Open();
            SqlCommand command = new($@"SELECT h.*, CONCAT(h.Street, ' ', h.HouseNo, '- ', h.FlatNo) AS FullAddress, c.Name, CONCAT(b.Name, ' ', b.Surname) AS BrokerName
                                        FROM Houses h
                                        LEFT JOIN Companies c
                                        ON h.CompanyId = c.CompanyId
                                        LEFT JOIN Brokers b
                                        ON h.BrokerId = b.BrokerId
                                        WHERE c.CompanyId = {companyId};", _connection);
            using var Reader = command.ExecuteReader();
            while (Reader.Read())
            {
                companyAppartments.Add(new()
                {
                    FlatId = Reader.GetInt32(0),
                    City = Reader.GetString(1),
                    Street = Reader.GetString(2),
                    HouseNo = Reader.GetString(3),
                    FlatNo = Reader.GetString(4),
                    FlatFloor = Reader.GetInt32(5),
                    BuildingFloors = Reader.GetInt32(6),
                    Area = Reader.GetDecimal(7),
                    //BrokerId = Reader.GetInt32(8),
                    CompanyId = Reader.GetInt32(9),
                    FullAddress = Reader.GetString(10),
                    CompanyName = Reader.GetString(11),
                    Broker = Reader.GetString(12)
                });
            }
            _connection.Close();
            return companyAppartments;
        }

    }
}
