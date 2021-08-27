using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPAPP.Model;

namespace ERPAPP.Logic
{
    class DataAcess
    {
        public static List<tblUser> Getusers()
        {
            List<tblUser> list;

            //using : 범위 이외에는 자동으로 dispose하여 용량에 문제가 없도록 한다.
            //ERPEntities()의 유저테이블 리스트를 전달 받음
            using(var ctx = new ERPEntities())
            {
                list = ctx.tblUser.ToList();
            } 

            return list;
        }

        internal static List<tblReport> GetReport(string production)
        {
            var connString = ConfigurationManager.ConnectionStrings["ERPConnString"].ToString();
            List<tblReport> list = new List<tblReport>();
            var lastObj = new tblReport();

            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                var sqlQuery = $@" select Date,COUNT(*) as '전체 수량' from tblMES where ProductionCode = '{production}' group by Date";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var tmp = new Model.tblReport
                    {
                        Date = DateTime.Parse(reader["Date"].ToString()),
                        Quantity = (int)reader["전체 수량"]
                    };
                    list.Add(tmp);
                    lastObj = tmp; // 마지막 값을 할당
                }

                return list;
            }
        }

        /*public static int SetUsers(tblUser item)
        {
            using (var ctx = new ERPEntities())
            {
                ctx.tblUser.AddOrUpdate(item);
                return ctx.SaveChanges(); // commit
            }
        }*/

        public static List<tblItem> GetItems()
        {
            List<tblItem> list;

            using (var ctx = new ERPEntities())
            {
                list = ctx.tblItem.ToList();
            }

            return list;
        }

        public static int SetItems(tblItem item)
        {
            using (var ctx = new ERPEntities())
            {
                ctx.tblItem.AddOrUpdate(item);
                return ctx.SaveChanges(); // commit
            }
        }

        public static List<tblBrand> GetBrands()
        {
            List<tblBrand> list;

            using (var ctx = new ERPEntities())
            {
                list = ctx.tblBrand.ToList();
            }

            return list;
        }
        public static int SetBrands(tblBrand item)
        {
            using (var ctx = new ERPEntities())
            {
                ctx.tblBrand.AddOrUpdate(item);
                return ctx.SaveChanges(); // commit
            }
        }

        public static List<tblICate> GetICates()
        {
            List<tblICate> list;

            using (var ctx = new ERPEntities())
            {
                list = ctx.tblICate.ToList();
            }

            return list;
        }

        public static int SetICates(tblICate item)
        {
            using (var ctx = new ERPEntities())
            {
                ctx.tblICate.AddOrUpdate(item);
                return ctx.SaveChanges(); // commit
            }
        }

        public static List<tblBOM> GetBOM()
        {
            List<tblBOM> list;

            using (var ctx = new ERPEntities())
            {
                list = ctx.tblBOM.ToList();
            }

            return list;
        }

        public static int SetBOM(tblBOM item)
        {
            using (var ctx = new ERPEntities())
            {
                ctx.tblBOM.AddOrUpdate(item);
                return ctx.SaveChanges(); // commit
            }
        }

        internal static List<tblOrder> GetOrders()
        {
            List<tblOrder> list;

            using (var ctx = new ERPEntities())
            {
                list = ctx.tblOrder.ToList();
            }

            return list;
        }

        public static int SetOrders(tblOrder item)
        {
            using (var ctx = new ERPEntities())
            {
                ctx.tblOrder.AddOrUpdate(item);
                return ctx.SaveChanges(); // commit
            }
        }

        internal static List<tblFactory> GetFactorys()
        {
            List<tblFactory> list;

            using (var ctx = new ERPEntities())
            {
                list = ctx.tblFactory.ToList();
            }

            return list;
        }

        public static int SetFactorys(tblFactory item)
        {
            using (var ctx = new ERPEntities())
            {
                ctx.tblFactory.AddOrUpdate(item);
                return ctx.SaveChanges(); // commit
            }
        }

        internal static List<tblWorker> GetWorkers()
        {
            List<tblWorker> list;

            using (var ctx = new ERPEntities())
            {
                list = ctx.tblWorker.ToList();
            }

            return list;
        }

        public static int SetWorkers(tblWorker item)
        {
            using (var ctx = new ERPEntities())
            {
                ctx.tblWorker.AddOrUpdate(item);
                return ctx.SaveChanges(); // commit
            }
        }

        internal static List<tblMachine> GetMachines()
        {
            List<tblMachine> list;

            using (var ctx = new ERPEntities())
            {
                list = ctx.tblMachine.ToList();
            }

            return list;
        }

        public static int SetMachines(tblMachine item)
        {
            using (var ctx = new ERPEntities())
            {
                ctx.tblMachine.AddOrUpdate(item);
                return ctx.SaveChanges(); // commit
            }
        }

        internal static List<tblMcCate> GetMcCates()
        {
            List<tblMcCate> list;

            using (var ctx = new ERPEntities())
            {
                list = ctx.tblMcCate.ToList();
            }

            return list;
        }

        public static int SetMcCates(tblMcCate item)
        {
            using (var ctx = new ERPEntities())
            {
                ctx.tblMcCate.AddOrUpdate(item);
                return ctx.SaveChanges(); // commit
            }
        }

        internal static List<tblProduction> GetProductions()
        {
            List<tblProduction> list;

            using (var ctx = new ERPEntities())
            {
                list = ctx.tblProduction.ToList();
            }

            return list;
        }

        internal static List<tblProduction> GetNowProduction()
        {
            var connString = ConfigurationManager.ConnectionStrings["ERPConnString"].ToString();
            List<tblProduction> list = new List<tblProduction>();
            var lastObj = new tblProduction();

            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                var sqlQuery = $@"select * from tblProduction where StartDate < GETDATE() and EndDate > GETDATE()";

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var tmp = new Model.tblProduction
                    {
                        ProductionId = (int)reader["ProductionId"],
                        ProductionCode = reader["ProductionCode"].ToString(),
                        FactoryCode = reader["FactoryCode"].ToString().Trim(),
                        ItemCode = reader["ItemCode"].ToString(),
                        OrderCode = reader["OrderCode"].ToString(),
                        StartDate = (DateTime)reader["StartDate"],
                        EndDate = (DateTime)reader["EndDate"],
                        PlanQuantity = (int)reader["PlanQuantity"],
                        FQuantity = (int)reader["FQuantity"]
                    };
                    list.Add(tmp);
                    lastObj = tmp; // 마지막 값을 할당
                }

                return list;
            }
        }

        public static int SetProductions(tblProduction item)
        {
            using (var ctx = new ERPEntities())
            {
                ctx.tblProduction.AddOrUpdate(item);
                return ctx.SaveChanges(); // commit
            }
        }

        internal static List<tblOperation> GetOperations()
        {
            List<tblOperation> list;

            using (var ctx = new ERPEntities())
            {
                list = ctx.tblOperation.ToList();
            }

            return list;
        }

        public static int SetOperations(tblOperation item)
        {
            using (var ctx = new ERPEntities())
            {
                ctx.tblOperation.AddOrUpdate(item);
                return ctx.SaveChanges(); // commit
            }
        }

        internal static List<tblMES> GetMES()
        {
            List<tblMES> list;

            using (var ctx = new ERPEntities())
            {
                list = ctx.tblMES.ToList();
            }

            return list;
        }

        public static int SetMES(tblMES item)
        {
            using (var ctx = new ERPEntities())
            {
                ctx.tblMES.AddOrUpdate(item);
                return ctx.SaveChanges(); // commit
            }
        }
    }
}
