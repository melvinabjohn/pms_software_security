using System;
using System.Data;
using AutoMapper;
using Test1.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Test1.Data
{

    //public interface IPMSSQLStorageContext
    //{
    //    PMSDbContext GetSqlDbContext();
    //    ReadDataProvider GetDataReadProvider();
    //}
    //public class PMSSQLStorageContext : IPMSSQLStorageContext
    //{
    //    readonly IDataProvider _dataProvider;

    //    private readonly IMapper _mapper;

    //    private readonly PMSDbContext _dbContext;
    //    public PMSSQLStorageContext(PMSDbContext dbContext, IMapper mapper)
    //    {
    //        _mapper = mapper;
    //        _dbContext = dbContext;
    //        _dataProvider = new SqlDataReadProvider(dbContext.Database.GetDbConnection().ConnectionString);
    //    }


    //    public PMSDbContext GetSqlDbContext()
    //    {
    //        return _dbContext;
    //    }

    //    public virtual ReadDataProvider GetDataReadProvider()
    //    {
    //        return new ReadDataProvider(_dataProvider, _mapper);
    //    }
    //}

}

