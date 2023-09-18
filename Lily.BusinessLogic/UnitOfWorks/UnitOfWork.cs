using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServiceLibrary.EmailService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.DAL.Data;
using TaskManager.DomainLayer.Models;
using TaskManager.BusinessLogic.Repositories;

namespace TaskManger.BusinessLogic.UnitOfWorks
{
	/// <summary>
	/// Task Manager Respository
	/// </summary>
	/// <remarks>Warehouses all the respository in solution so that they can be accessed from a single point</remarks>
	public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private UserRepo? _users;
        private RoleRepo? _roles;
        private UserTaskRepo? _userTaskRepo;
        private ProjectRepo? _projectRepo;
        private NotificationRepo? _notification;
        private AssignedUserTaskRepo? _assignedUserTask;



		#region
		public UnitOfWork(ApplicationDbContext context, IMapper mapper,
            UserManager<User> userManager, IEmailService emailService, 
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _emailService = emailService;
            _roleManager = roleManager;
           
        }
		#endregion


		public UserRepo User => _users ??= new UserRepo(_userManager, _mapper, _emailService);
        public RoleRepo Role => _roles ??= new RoleRepo(_roleManager);

        public UserTaskRepo UserTask => _userTaskRepo ??= new UserTaskRepo(_context, _mapper, _emailService);
        public ProjectRepo Project => _projectRepo ??= new ProjectRepo(_context, _mapper);
        public NotificationRepo Notification => _notification ??= new NotificationRepo(_context, _mapper, _emailService);
        public AssignedUserTaskRepo AssignedUserTask => _assignedUserTask ??= new AssignedUserTaskRepo(_context, _mapper, _emailService);

		public async Task Complete()
		{
			await _context.SaveChangesAsync();
		}

		public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

		
	}
}
