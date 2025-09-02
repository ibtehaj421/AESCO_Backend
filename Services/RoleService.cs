using System.Formats.Asn1;
using Microsoft.AspNetCore.Http.HttpResults;
using ASCO.Models;
using ASCO.Repositories;
using Microsoft.AspNetCore.Identity;
using ASCP.Repositories;

namespace ASCO.Services
{
    public class RoleService
    {
        private readonly RoleRepository _roleRepository;

        public RoleService(RoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<string> CreateMasterRoles()
        {
            try
            {
                await CreatePermissionsAsync();
                await CreateRolesAsync();
                await AssignPermissionsToRolesAsync();
                return "Master roles and permissions created successfully.";
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return $"Error creating master roles: {ex.Message}";
            }
        }

        //permissions
        // Method to create permissions first (they're the foundation)
        public async Task CreatePermissionsAsync()
        {
            // Check if permissions already exist to avoid duplicates
            if (await _roleRepository.AnyPermissionAsync())
            {
                Console.WriteLine("Permissions already exist, skipping creation...");
                return;
            }

            var permissions = new List<Permission>
            {
                // User Management Permissions
                new Permission { Name = "Users.Create", Description = "Create new users", Module = "Users" },
                new Permission { Name = "Users.Read", Description = "View user information", Module = "Users" },
                new Permission { Name = "Users.Update", Description = "Update user information", Module = "Users" },
                new Permission { Name = "Users.Delete", Description = "Delete users", Module = "Users" },
                new Permission { Name = "Users.ManageRoles", Description = "Assign/remove user roles", Module = "Users" },
                
                // Role Management Permissions
                new Permission { Name = "Roles.Create", Description = "Create new roles", Module = "Roles" },
                new Permission { Name = "Roles.Read", Description = "View roles", Module = "Roles" },
                new Permission { Name = "Roles.Update", Description = "Update role information", Module = "Roles" },
                new Permission { Name = "Roles.Delete", Description = "Delete roles", Module = "Roles" },
                new Permission { Name = "Roles.ManagePermissions", Description = "Assign/remove role permissions", Module = "Roles" },
                
                // Reports Permissions
                new Permission { Name = "Reports.View", Description = "View reports", Module = "Reports" },
                new Permission { Name = "Reports.Export", Description = "Export report data", Module = "Reports" },
                new Permission { Name = "Reports.Create", Description = "Create custom reports", Module = "Reports" },
                
                // System Administration
                new Permission { Name = "System.Settings", Description = "Manage system settings", Module = "System" },
                new Permission { Name = "System.Audit", Description = "View system audit logs", Module = "System" },
                new Permission { Name = "System.Backup", Description = "Perform system backups", Module = "System" },
                
                // Profile Management
                new Permission { Name = "Profile.View", Description = "View own profile", Module = "Profile" },
                new Permission { Name = "Profile.Update", Description = "Update own profile", Module = "Profile" },
                new Permission { Name = "Profile.ChangePassword", Description = "Change own password", Module = "Profile" }
            };

            var check = await _roleRepository.AddPermissionsAsync(permissions);
            if (check <= 0)
            {
                Console.WriteLine("Failed to create permissions.");
            }
            Console.WriteLine($"Created {permissions.Count} permissions");

            // Show what gets stored in Permissions table
            // foreach (var permission in permissions)
            // {
            //     Console.WriteLine($"Permission ID: {permission.Id}, Name: {permission.Name}, Module: {permission.Module}");
            // }
        }

        public async Task CreateRolesAsync()
        {
            // Check if roles already exist
            if (await _roleRepository.AnyRolesAsync())
            {
                Console.WriteLine("Roles already exist, skipping creation...");
                return;
            }

            var roles = new List<Role>
            {
                new Role { Name = "SuperAdmin", Description = "Full system access", IsActive = true },
                new Role { Name = "Admin", Description = "Administrative access", IsActive = true },
                new Role { Name = "Manager", Description = "Management level access", IsActive = true },
                new Role { Name = "Employee", Description = "Standard employee access", IsActive = true },
                new Role { Name = "Developer", Description = "Development team access", IsActive = true },
                new Role { Name = "HR", Description = "Human resources access", IsActive = true },
                new Role { Name = "Contractor", Description = "Temporary contractor access", IsActive = true }
            };

            var check = await _roleRepository.AddRolesAsync(roles);
            if (check <= 0)
            {
                Console.WriteLine("Failed to create roles.");
            }

            Console.WriteLine($"Created {roles.Count} roles");

            // Show what gets stored in Roles table
            foreach (var role in roles)
            {
                Console.WriteLine($"Role ID: {role.Id}, Name: {role.Name}, Description: {role.Description}");
            }
        }

        public async Task AssignPermissionsToRolesAsync()
        {
            // Load existing roles and permissions
            var roles = await _roleRepository.RolesToListAsync();
            var permissions = await _roleRepository.PermissionsToListAsync();

            if (!roles.Any() || !permissions.Any())
            {
                throw new InvalidOperationException("Roles and permissions must be created first");
            }

            // Create permission mappings for each role
            var rolePermissions = new List<RolePermission>();

            // SuperAdmin - ALL permissions
            var superAdminRole = roles.First(r => r.Name == "SuperAdmin");
            foreach (var permission in permissions)
            {
                rolePermissions.Add(new RolePermission
                {
                    RoleId = superAdminRole.Id,
                    PermissionId = permission.Id,
                    GrantedAt = DateTime.UtcNow
                });
            }

            // Admin - Most permissions except system-level ones
            var adminRole = roles.First(r => r.Name == "Admin");
            var adminPermissionNames = new[]
            {
            "Users.Create", "Users.Read", "Users.Update", "Users.Delete", "Users.ManageRoles",
            "Roles.Read", "Reports.View", "Reports.Export", "Reports.Create",
            "Profile.View", "Profile.Update", "Profile.ChangePassword"
        };
            foreach (var permissionName in adminPermissionNames)
            {
                var permission = permissions.First(p => p.Name == permissionName);
                rolePermissions.Add(new RolePermission
                {
                    RoleId = adminRole.Id,
                    PermissionId = permission.Id,
                    GrantedAt = DateTime.UtcNow
                });
            }

            // Manager - User management and reports
            var managerRole = roles.First(r => r.Name == "Manager");
            var managerPermissionNames = new[]
            {
            "Users.Read", "Users.Update", "Reports.View", "Reports.Export",
            "Profile.View", "Profile.Update", "Profile.ChangePassword"
        };
            foreach (var permissionName in managerPermissionNames)
            {
                var permission = permissions.First(p => p.Name == permissionName);
                rolePermissions.Add(new RolePermission
                {
                    RoleId = managerRole.Id,
                    PermissionId = permission.Id,
                    GrantedAt = DateTime.UtcNow
                });
            }

            // Employee - Basic access
            var employeeRole = roles.First(r => r.Name == "Employee");
            var employeePermissionNames = new[]
            {
            "Users.Read", "Reports.View", "Profile.View", "Profile.Update", "Profile.ChangePassword"
        };
            foreach (var permissionName in employeePermissionNames)
            {
                var permission = permissions.First(p => p.Name == permissionName);
                rolePermissions.Add(new RolePermission
                {
                    RoleId = employeeRole.Id,
                    PermissionId = permission.Id,
                    GrantedAt = DateTime.UtcNow
                });
            }

            // Developer - Development specific permissions
            var developerRole = roles.First(r => r.Name == "Developer");
            var developerPermissionNames = new[]
            {
            "Users.Read", "Reports.View", "Reports.Create", "System.Audit",
            "Profile.View", "Profile.Update", "Profile.ChangePassword"
        };
            foreach (var permissionName in developerPermissionNames)
            {
                var permission = permissions.First(p => p.Name == permissionName);
                rolePermissions.Add(new RolePermission
                {
                    RoleId = developerRole.Id,
                    PermissionId = permission.Id,
                    GrantedAt = DateTime.UtcNow
                });
            }

            // HR - Human resources permissions
            var hrRole = roles.First(r => r.Name == "HR");
            var hrPermissionNames = new[]
            {
            "Users.Create", "Users.Read", "Users.Update", "Users.ManageRoles",
            "Reports.View", "Reports.Export", "Profile.View", "Profile.Update", "Profile.ChangePassword"
        };
            foreach (var permissionName in hrPermissionNames)
            {
                var permission = permissions.First(p => p.Name == permissionName);
                rolePermissions.Add(new RolePermission
                {
                    RoleId = hrRole.Id,
                    PermissionId = permission.Id,
                    GrantedAt = DateTime.UtcNow
                });
            }

            // Contractor - Limited access
            var contractorRole = roles.First(r => r.Name == "Contractor");
            var contractorPermissionNames = new[]
            {
            "Reports.View", "Profile.View", "Profile.Update", "Profile.ChangePassword"
        };
            foreach (var permissionName in contractorPermissionNames)
            {
                var permission = permissions.First(p => p.Name == permissionName);
                rolePermissions.Add(new RolePermission
                {
                    RoleId = contractorRole.Id,
                    PermissionId = permission.Id,
                    GrantedAt = DateTime.UtcNow
                });
            }

            // Save all role-permission assignments
           var check = await _roleRepository.AddRolePermissionsAsync(rolePermissions);
            if (check <= 0)
            {
                Console.WriteLine("Failed to assign permissions to roles.");
            }

            Console.WriteLine($"Created {rolePermissions.Count} role-permission assignments");

            // Show what gets stored in RolePermissions table
            foreach (var rolePermission in rolePermissions.Take(10)) // Show first 10 as example
            {
                var role = roles.First(r => r.Id == rolePermission.RoleId);
                var permission = permissions.First(p => p.Id == rolePermission.PermissionId);
                Console.WriteLine($"RolePermission: RoleID={rolePermission.RoleId}({role.Name}), " +
                                $"PermissionID={rolePermission.PermissionId}({permission.Name}), " +
                                $"GrantedAt={rolePermission.GrantedAt}");
            }
        }
    
    //     // Method to create a custom role with specific permissions
    // public async Task<Role> CreateCustomRoleAsync(string roleName, string description, List<string> permissionNames)
    // {
    //     // Create the role
    //     var role = new Role
    //     {
    //         Name = roleName,
    //         Description = description,
    //         IsActive = true,
    //         CreatedAt = DateTime.UtcNow
    //     };

    //     _context.Roles.Add(role);
    //     await _context.SaveChangesAsync(); // Get the generated ID

    //     // Assign permissions
    //     var permissions = await _context.Permissions
    //         .Where(p => permissionNames.Contains(p.Name))
    //         .ToListAsync();

    //     var rolePermissions = permissions.Select(p => new RolePermission
    //     {
    //         RoleId = role.Id,
    //         PermissionId = p.Id,
    //         GrantedAt = DateTime.UtcNow
    //     }).ToList();

    //     _context.RolePermissions.AddRange(rolePermissions);
    //     await _context.SaveChangesAsync();

    //     Console.WriteLine($"Created custom role '{roleName}' with {rolePermissions.Count} permissions");
        
    //     return role;
    // }
    }
}