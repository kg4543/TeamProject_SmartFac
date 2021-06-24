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
            List<tblUser> users;

            //using : 범위 이외에는 자동으로 dispose하여 용량에 문제가 없도록 한다.
            //ERPEntities()의 유저테이블 리스트를 전달 받음
            using(var ctx = new ERPEntities())
            {
                users = ctx.tblUser.ToList();
            } 

            return users;
        }
        public static int SetUsers(tblUser user)
        {
            using (var ctx = new ERPEntities())
            {
                ctx.tblUser.AddOrUpdate(user);
                return ctx.SaveChanges(); // commit
            }
        }

        public static List<tblItem> GetItems()
        {
            List<tblItem> items;

            using (var ctx = new ERPEntities())
            {
                items = ctx.tblItem.ToList();
            }

            return items;
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
            List<tblBrand> brands;

            using (var ctx = new ERPEntities())
            {
                brands = ctx.tblBrand.ToList();
            }

            return brands;
        }
        public static int SetBrands(tblBrand brand)
        {
            using (var ctx = new ERPEntities())
            {
                ctx.tblBrand.AddOrUpdate(brand);
                return ctx.SaveChanges(); // commit
            }
        }

        public static List<tblICate> GetICates()
        {
            List<tblICate> iCates;

            using (var ctx = new ERPEntities())
            {
                iCates = ctx.tblICate.ToList();
            }

            return iCates;
        }
        public static int SetBrands(tblICate iCate)
        {
            using (var ctx = new ERPEntities())
            {
                ctx.tblICate.AddOrUpdate(iCate);
                return ctx.SaveChanges(); // commit
            }
        }
    }
}
