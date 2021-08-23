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

        public void SaveBrokerToCompany(CompanyCreateModel selectedCompany)
        {
            foreach (int entry in selectedCompany.BrokerIds)
            {
                _connection.Open();

                SqlCommand command = new($@"INSERT INTO CompanyBroker (CompanyId, BrokerId) 
                                VALUES ('{selectedCompany.Company.CompanyId}', '{entry}');", _connection);
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

    }
}
