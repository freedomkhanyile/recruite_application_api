using System;

namespace CourseApplications.DAL.UnitOfWork
{
    public interface ITransaction : IDisposable
    {
        void Commit();
        void Rollback();
    }
}