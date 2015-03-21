using StackOverflow.Domain.Entities;

namespace StackOverflow.Data
{
    public interface IUnitOfWork
    {

        void Commit();

        void Rollback();

        GenericRepository<Account> AccountRepository { get; }

        GenericRepository<Question> QuestionRepository { get; }

        GenericRepository<Answer> AnswerRepository { get; }

        GenericRepository<Comment> CommentRepository { get; }
    }
}
