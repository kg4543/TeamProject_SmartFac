using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
        public static int SetUsers(tblUser item)
        {
            using (var ctx = new ERPEntities())
            {
                ctx.tblUser.AddOrUpdate(item);
                return ctx.SaveChanges(); // commit
            }
        }

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
    }
}
