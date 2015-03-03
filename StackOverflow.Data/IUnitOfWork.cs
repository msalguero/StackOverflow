namespace StackOverflow.Data
{
    interface IUnitOfWork
    {
        void Commit();

        void Rollback();
    }
}
