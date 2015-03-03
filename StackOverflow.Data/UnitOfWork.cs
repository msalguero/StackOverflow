﻿using StackOverflow.Domain.Entities;

namespace StackOverflow.Data
{
    class UnitOfWork : IUnitOfWork
    {
        private readonly StackOverflowContext _context;
        private GenericRepository<Account> _accountRepository;
        private GenericRepository<Question> _questionRepository;
        private GenericRepository<Answer> _answerRepository; 

        public UnitOfWork()
        {
            _context = new StackOverflowContext();
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Rollback()
        {
            _context.Dispose();
        }

        public GenericRepository<Account> AccountRepository
        {
            get { return _accountRepository ?? (_accountRepository = new GenericRepository<Account>(_context)); }
        }

        public GenericRepository<Question> QuestionRepository
        {
            get { return _questionRepository ?? (_questionRepository = new GenericRepository<Question>(_context)); }
        }

        public GenericRepository<Answer> AnswerRepository
        {
            get { return _answerRepository ?? (_answerRepository = new GenericRepository<Answer>(_context)); }
        } 
    }
}