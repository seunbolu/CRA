using CRA.Data.Conventions;
using CRA.Data.Entities;
using CRA.Data.Tracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data
{
    public class DataContext : DbContext
    {


        public long? ChangeSetId { get; set; }



        #region "Core Entities"


        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<CHGSite> CHGSites { get; set; }
        public virtual DbSet<ReferralSource> ReferralSources { get; set; }
        public virtual DbSet<CHGSiteReferralSource> CHGSiteReferralSources { get; set; }
        public virtual DbSet<Content> Contents { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<ContactAttribute> ContactAttributes { get; set; }
        public virtual DbSet<ContactReferralSource> ContactReferralSources { get; set; }
        public virtual DbSet<Referral> Referrals { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Email> Emails { get; set; }
        public virtual DbSet<EmailTo> EmailTos { get; set; }
        public virtual DbSet<EmailReplyTo> EmailReplyTos { get; set; }
        public virtual DbSet<EmailCc> EmailCcs { get; set; }
        public virtual DbSet<EmailBcc> EmailBccs { get; set; }
        public virtual DbSet<EmailAttachment> EmailAttachments { get; set; }
        public virtual DbSet<ReferralSourceElectronicReferralType> ReferralSourceElectronicReferralTypes { get; set; }
        public virtual DbSet<ReferralSourceCommercialPayorType> ReferralSourceCommercialPayorTypes { get; set; }
        public virtual DbSet<ReferralSourceManagedMedicarePayorType> ReferralSourceManagedMedicarePayorTypes { get; set; }
        public virtual DbSet<CommercialPayorType> CommercialPayorTypes { get; set; }
        public virtual DbSet<ManagedMedicarePayorType> ManagedMedicarePayorTypes { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<OrganizationServiceType> OrganizationServiceTypes { get; set; }
        public virtual DbSet<PreScreen> PreScreens { get; set; }
        public virtual DbSet<PreScreenData> PreScreenData { get; set; }
        public virtual DbSet<PatientData> PatientData { get; set; }
        public virtual DbSet<ChangeSet> ChangeSets { get; set; }
        public virtual DbSet<ChangeHistory> ChangeHistories { get; set; }

        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<ScheduleItem> ScheduleItems { get; set; }

        public virtual DbSet<UserTaskParameter> UserTaskParameters { get; set; }

        public virtual DbSet<Template> Templates { get; set; }

        public virtual DbSet<Notification> Notifications { get; set; }

        public virtual DbSet<UserNotification> UserNotifications { get; set; }

        public virtual DbSet<ScheduleVisit> ScheduleVisits { get; set; }

        #endregion


        #region "List lookup fields"
        public virtual DbSet<ServiceType> ServiceTypes { get; set; }
        public virtual DbSet<RegionType> RegionTypes { get; set; }
        public virtual DbSet<SpecialityType> SpecialityTypes { get; set; }
        public virtual DbSet<PreScreenType> PreScreenTypes { get; set; }
        public virtual DbSet<ContactRoleType> ContactRoleTypes { get; set; }
        public virtual DbSet<CategoryType> CategoryTypes { get; set; }
        public virtual DbSet<RegionServiceType> RegionServiceTypes { get; set; }
        public virtual DbSet<PreScreenUpdateType> PreScreenUpdateTypes { get; set; }
        public virtual DbSet<ReferralSourceType> ReferralSourceTypes { get; set; }
        public virtual DbSet<UserRoleType> UserRoleTypes { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<UserService> UserServices { get; set; }
        public virtual DbSet<UserRegion> UserRegions { get; set; }
        public virtual DbSet<UserCHGSite> UserCHGSites { get; set; }
        public virtual DbSet<UserPreScreen> UserPreScreens { get; set; }
     
        public virtual DbSet<ElectronicReferralType> ElectronicReferralTypes { get; set; }


        public virtual DbSet<GenderType> GenderTypes { get; set; }

        public virtual DbSet<StateType> StateTypes { get; set; }

        public virtual DbSet<CountryType> CountryTypes { get; set; }


        public virtual DbSet<EthnicityType> EthnicityTypes { get; set; }

        public virtual DbSet<MaritalStatusType> MaritalStatusTypes { get; set; }
        public virtual DbSet<LanguageType> LanguageTypes { get; set; }
        public virtual DbSet<RelationshipType> RelationshipTypes { get; set; }
        public virtual DbSet<EmailStatusType> EmailStatusTypes { get; set; }
        public virtual DbSet<UserTask> UserTasks { get; set; }

        public virtual DbSet<UnplannedVisit> UnplannedVisits { get; set; }

        public virtual DbSet<PreScreenStatusLog> PreScreenStatusLogs { get; set; }

        

        #endregion

        long? _userId;

        public long? UserId { get { return _userId; } }

        public DataContext() : this("CRAConnectionString")
        {

        }
        public DataContext(long userId) : this("CRAConnectionString")
        {
            _userId = userId;
        }

        public DataContext(string connectionStringName) : base(connectionStringName)
        {

        }

        public DataContext(long userId, string connectionStringName) : base(connectionStringName)
        {
            _userId = userId;
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Properties<string>().Configure(p => p.IsUnicode(false));
            modelBuilder.Conventions.Add(new DataTypePropertyAttributeConvention());
            modelBuilder.HasDefaultSchema("Core");

            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

        }


        T UnProxy<T>(DbContext context, T proxyObject) where T : class
        {
            var proxyCreationEnabled = context.Configuration.ProxyCreationEnabled;
            try
            {
                context.Configuration.ProxyCreationEnabled = false;
                T poco = context.Entry(proxyObject).OriginalValues.ToObject() as T;
                return poco;
            }
            finally
            {
                context.Configuration.ProxyCreationEnabled = proxyCreationEnabled;
            }
        }


        public override int SaveChanges()
        {
            foreach (var entry in this.ChangeTracker.Entries())
            {
                if (entry.Entity is EntityAuditBase)
                {
                    var entity = (EntityAuditBase)entry.Entity;
                    if (entry.State == EntityState.Added)
                    {
                        entity.DateCreated = DateTime.Now;
                    }

                    if (entry.State == EntityState.Modified)
                    {
                        entity.DateModified = DateTime.Now;
                    }

                }

                if (entry.Entity is EntityBase)
                {
                    var entity = (EntityBase)entry.Entity;
                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedByUserId = _userId;
                    }

                    if (entry.State == EntityState.Modified)
                    {
                        entity.ModifiedByUserId = _userId;
                    }

                }


                if (ChangeSetId != null && (entry.State == EntityState.Modified || entry.State == EntityState.Deleted) && !(entry.Entity is ChangeHistory) && !(entry.Entity is ChangeSet))
                {
                    long id = 0;
                    foreach (var property in entry.OriginalValues.PropertyNames)
                    {

                        bool isNotTracked = false;

                        var prop = entry.Entity.GetType().GetProperty(property);
                        var attrs = prop.GetCustomAttributes(true);
                        foreach (var attr in attrs)
                        {
                            if (attr.GetType() == typeof(NotTrackedAttribute))
                            {
                                isNotTracked = true;

                            }

                            if (attr.GetType() == typeof(KeyAttribute))
                            {
                                id = entry.OriginalValues.GetValue<long>(property);

                            }
                        }

                        var obj = UnProxy<object>(this, entry.Entity);

                        if (entry.Property(property).IsModified && !isNotTracked)
                        {
                            string operation = null;


                            string entity = obj.GetType().Name;
                            string field = property;
                            long entityKey = id;

                            object newValue = entry.CurrentValues.GetValue<object>(property);
                            object oldValue = null;

                            if (entry.State != EntityState.Added)
                            {
                                oldValue = entry.OriginalValues.GetValue<object>(property);

                            }

                            switch (entry.State)
                            {
                                case EntityState.Added:
                                    operation = "Added";
                                    break;
                                case EntityState.Modified:
                                    operation = "Updated";
                                    break;
                                case EntityState.Deleted:
                                    operation = "Deleted";
                                    break;
                            }

                            if (entry.State == EntityState.Modified)
                            {
                                if (entry.CurrentValues.GetValue<bool>("Deleted") == true && entry.OriginalValues.GetValue<bool>("Deleted") == false)
                                {
                                    operation = "Deleted";
                                }
                            }


                            ChangeHistory history = new ChangeHistory();
                            history.ChangeSetId = ChangeSetId.Value;

                            history.NewValue = newValue == null ? null : newValue.ToString();
                            history.OldValue = oldValue?.ToString();
                            history.Operation = operation;
                            history.Entity = entity;
                            history.EntityKey = entityKey;
                            history.Field = field;

                            ChangeHistories.Add(history);

                        }


                    }

                }

            }

            return base.SaveChanges();
        }

        public void AddOrUpdatePatientData(long patientId, string itemType, string label, string sectionCode, string clientCode, string value, string dependsOnCode, string dependsOnValue)
        {
            var entity = PatientData.Where(p => p.SectionCode == sectionCode && p.ItemCode == clientCode && p.PatientId == patientId && p.Deleted == false).SingleOrDefault();
            if (entity == null)
            {
                entity = new PatientData();
            }

            entity.PatientId = patientId;
            entity.ItemCode = clientCode;
            entity.Type = itemType;
            entity.Value = value;
            entity.DependsOnCode = dependsOnCode;
            entity.DependsOnAssertValue = dependsOnValue;
            entity.Label = label;

        }
        public void AddOrUpdatePreScreenData(long preScreenId, string itemType, string label, string sectionCode, string clientCode, string value, string dependsOnCode, string dependsOnValue)
        {
            var entity = PreScreenData.Where(p => p.SectionCode == sectionCode && p.ItemCode == clientCode && p.PreScreenId == preScreenId && p.Deleted == false).SingleOrDefault();
            if (entity == null)
            {
                entity = new PreScreenData();
            }

            entity.PreScreenId = preScreenId;
            entity.ItemCode = clientCode;
            entity.Type = itemType;
            entity.Value = value;
            entity.DependsOnCode = dependsOnCode;
            entity.DependsOnAssertValue = dependsOnValue;
            entity.Label = label;

        }


        public void BeginChangeSet()
        {
            ChangeSet _changeSet = new ChangeSet();
            _changeSet.UserId = _userId.Value;
            _changeSet.ChangeDate = DateTime.Now;
            ChangeSets.Add(_changeSet);
            SaveChanges();
            ChangeSetId = _changeSet.ChangeSetId;
        }

        public void CompleteChangeSet()
        {
            ChangeSetId = null;
        }




        public void ApproveSchedule(long scheduleId)
        {
            var sch = Schedules.Find(scheduleId);
            var userId = sch.UserId;
            foreach (var schedule in Schedules.Where(p => p.UserId == userId && p.ActivationDate != null && p.ScheduleId != scheduleId).ToList())
            {
                schedule.DeactivationDate = DateTime.Now;
            }

            sch.Decided = true;
            sch.ActivationDate = DateTime.Now.AddDays(1);
        }


        public void ApproveReferralSource(long referralSourceId)
        {
            var entity = ReferralSources.Find(referralSourceId);
            entity.IsApproved = true;

        }


        

        public void RejectReferralSource(long referralSourceId)
        {
            var entity = ReferralSources.Find(referralSourceId);
            entity.IsApproved = false;
            entity.Deleted = true;
        }


        public void ApproveContact(long contactId)
        {
            var entity = Contacts.Find(contactId);
            entity.IsApproved = true;

        }

        public void RejectContact(long contactId)
        {
            var entity = Contacts.Find(contactId);
            entity.IsApproved = false;
            entity.Deleted = true;
        }



        public void RejectSchedule(long scheduleId)
        {
            var sch = Schedules.Find(scheduleId);
            sch.Decided = true;
            sch.ActivationDate = null;
            sch.DeactivationDate = null;


        }


        public UserTask CreateTask(string taskType, long userId, string status, string description, string notes =null, string userNotes = null)
        {
            UserTask task = new UserTask();
            task.TaskType = taskType;
            task.UserId = userId;
            task.Status = status;
            task.Description = description;
            task.Notes = notes;
            task.UserNotes = userNotes;
            UserTasks.Add(task);
            return task;
        }

        public UserTaskParameter UpdateTaskParameter(long userTaskId, string key, string value)
        {
            UserTaskParameter parameter = UserTaskParameters.Where(p => p.UserTaskId == userTaskId && p.Key == key && p.Deleted == false).SingleOrDefault();
            if (parameter == null)
            {
                parameter = new UserTaskParameter();
                UserTaskParameters.Add(parameter);
            }

            parameter.UserTaskId = userTaskId;
            parameter.Key = key;
            parameter.Value = value;
            return parameter;
        }
        public Dictionary<string, string> GetUserTaskParameters(long userTaskId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            foreach (var item in UserTaskParameters.Where(p => p.UserTaskId == userTaskId && p.Deleted == false))
            {
                parameters.Add(item.Key, item.Value);
            }

            return parameters;
        }

        public void UpdateTaskStatus(long userTaskId, string status)
        {
            UserTask task = UserTasks.Find(userTaskId);
            task.Status = status;
        }

        private string MergeTemplateValues(string template, Dictionary<string, string> values)
        {
            string retVal = template;

            foreach (var key in values.Keys)
            {
                retVal = retVal.Replace($"##{key}##", values[key]);
            }
            return retVal;
        }

        public void CreateEmailAlert(long userId, Dictionary<string, string> values)
        {
            var user = Users.Include("Contact").Where(p => p.UserId == userId).Single();
            var emailTo = user.Contact.Email;
            var subjectTemplateCode = values["TMP_SUBJECT"];
            var bodyTemplateCode = values["TMP_BODY"];


            var subjectTemplate = Templates.Where(p => p.Name == subjectTemplateCode && p.Deleted == false).Single().Value;
            var bodyTemplate = Templates.Where(p => p.Name == bodyTemplateCode && p.Deleted == false).Single().Value;

            Email email = new Email();
            email.FromAddress = values["EMAIL_FROM_ADDRESS"];
            email.FromName = values["EMAIL_FROM_NAME"];
            email.Subject = MergeTemplateValues(subjectTemplate, values);
            email.IsHtml = true;
            email.Body = MergeTemplateValues(bodyTemplate, values);
            email.EmailStatusTypeId = EmailStatusTypes.Where(p => p.Name == "Created").Single().EmailStatusTypeId;
            Emails.Add(email);
            EmailTo to = new EmailTo();
            to.Email = email;
            to.Name = $"{user.Contact.FirstName} {user.Contact.LastName}";
            to.Address = emailTo;
            EmailTos.Add(to);

        }

        public void CreateNotification(string category, string subject, string message, long userId)
        {
            Notification entity = new Notification();
            entity.UserId = userId;
            entity.Subject = subject;
            entity.Message = message;
            entity.Category = category;
            Notifications.Add(entity);
        }


        #region "Security"

        public bool UserHasRole(long userId, string role)
        {
            return UserRoles.Include("UserRoleType").Where(p => p.UserId == UserId && p.UserRoleType.Name == role).Count() > 0;
        }
        public List<User> GetRoleUsers(string role)
        {
            return UserRoles.Include("UserRoleType").Include("User").Include("User.Contact").Where(p => p.UserRoleType.Name == role && p.Deleted == false && p.User.Deleted == false && p.UserRoleType.Deleted == false).Select(p => p.User).ToList();
        }
        public UserRoleType GetRole(long roleId)
        {
            return UserRoleTypes.Find(roleId);
        }
        public UserRoleType GetRole(string role)
        {
            return UserRoleTypes.Where(p => p.Name == role && p.Deleted == false).SingleOrDefault();
        }
        public bool IsUserRoleForCHGSite(long userId, long CHGSiteId, long roleId)
        {

            var roleEntity = UserRoleTypes.Find(roleId);

            //Check for CRO
            if (roleEntity.Name == "CRO")
            {
                return true;
            }

            //Check for CAC
            if (roleEntity.Name == "CAC")
            {
                return true;
            }

            //Check for CEO and DBD
            if (roleEntity.Name == "CEO" || roleEntity.Name == "DBD")
            {
                return UserCHGSites.Where(p => p.UserId == UserId && p.Deleted == false && p.CHGSiteId == CHGSiteId).Count() > 0;
            }


            var items = (
              from os in OrganizationServiceTypes
              join rs in RegionServiceTypes on os.ServiceTypeId equals rs.ServiceTypeId
              join s in CHGSites on rs.RegionTypeId equals s.RegionTypeId
              where
              os.Deleted == false &&
              rs.Deleted == false &&
              s.Deleted == false
              select new
              {
                  OrganizationId = os.OrganizationId,
                  ServiceTypeId = os.ServiceTypeId,
                  RegionTypeId = rs.RegionTypeId,
                  CHGSiteId = s.CHGSiteId
              }).ToList();

            //Check for AVP
            if (roleEntity.Name == "AVP")
            {

                return (from c in UserRegions.Where(p => p.Deleted == false).ToList() join ur in UserRoles on c.UserId equals ur.UserId join i in items on c.RegionTypeId equals i.RegionTypeId where c.Deleted == false && c.UserId == userId && i.CHGSiteId == CHGSiteId && ur.User.Deleted == false && ur.User.Enabled == true && ur.UserRoleTypeId == roleId select c).Count() > 0;
            }

            //Add more permission checks here.

            return false;
        }
        public bool IsUserRoleForCHGSite(long userId, long CHGSiteId, string role)
        {
            var roleEntity = GetRole(role);
            var roleId = roleEntity.UserRoleTypeId;
            return IsUserRoleForCHGSite(userId, CHGSiteId, roleId);
        }
        public List<User> GetRoleUsersForCHGSite(long CHGSiteId, long roleId)
        {

            var roleEntity = UserRoleTypes.Find(roleId);

            //Check for CRO
            if (roleEntity.Name == "CRO")
            {
                return UserRoles.Include("UserRoleType").Include("User").Where(p => p.Deleted == false && p.UserRoleType.Name == "CRO" && p.User.Deleted == false && p.User.Enabled == true).Select(p => p.User).ToList();
            }

            if (roleEntity.Name == "CAC")
            {
                return UserRoles.Include("UserRoleType").Include("User").Where(p => p.Deleted == false && p.UserRoleType.Name == "CAC" && p.User.Deleted == false && p.User.Enabled == true).Select(p => p.User).ToList();
            }


            if (roleEntity.Name == "CEO" || roleEntity.Name == "DBD")
            {
                return (from ur in UserRoles join us in UserCHGSites on ur.UserId equals us.UserId where us.Deleted == false && ur.Deleted == false && ur.UserRoleTypeId == roleId && us.CHGSiteId == CHGSiteId && us.User.Deleted == false && us.User.Enabled == true select us.User).Distinct().ToList();
            }


            var items = (
           from os in OrganizationServiceTypes
           join rs in RegionServiceTypes on os.ServiceTypeId equals rs.ServiceTypeId
           join s in CHGSites on rs.RegionTypeId equals s.RegionTypeId
           where
           os.Deleted == false &&
           rs.Deleted == false &&
           s.Deleted == false
           select new
           {
               OrganizationId = os.OrganizationId,
               ServiceTypeId = os.ServiceTypeId,
               RegionTypeId = rs.RegionTypeId,
               CHGSiteId = s.CHGSiteId
           }).ToList();


            if (roleEntity.Name == "AVP")
            {
                return (from u in UserRegions.Where(p => p.Deleted == false).ToList() join ur in UserRoles on u.UserId equals ur.UserId join i in items on u.RegionTypeId equals i.RegionTypeId where u.Deleted == false && i.CHGSiteId == CHGSiteId && ur.Deleted == false && ur.User.Deleted == false && ur.User.Enabled == true && ur.UserRoleTypeId == roleId select u.User).Distinct().ToList();
            }

            return null;
        }
        public List<User> GetRoleUsersForCHGSite(long CHGSiteId, string role)
        {
            var roleEntity = GetRole(role);
            var roleId = roleEntity.UserRoleTypeId;

            return GetRoleUsersForCHGSite(CHGSiteId, roleId);
        }

        public List<UserRoleType> GetUserRoles(long userId)
        {
            return UserRoles.Include("UserRoleType").Include("User").Where(p => p.UserId == userId && p.Deleted == false && p.UserRoleType.Deleted == false && p.User.Deleted == false).Select(p => p.UserRoleType).ToList();
        }

        public List<CHGSite> GetUserSites(long userId)
        {
            List<CHGSite> sites = new List<CHGSite>();

            foreach (var role in GetUserRoles(userId))
            {
                switch (role.Name)
                {
                    case "CRO":
                        sites.AddRange(CHGSites.Where(p => p.Deleted == false).ToList());
                        break;
                    case "CAC":
                        sites.AddRange(CHGSites.Where(p => p.Deleted == false).ToList());
                        break;
                    case "SLH":
                        sites.AddRange((from c in UserServices.Where(p => p.Deleted == false && p.UserId == userId) join d in CHGSites.Where(p => p.Deleted == false) on c.ServiceTypeId equals d.ServiceTypeId select d).ToList());
                        break;
                    case "AVP":
                        sites.AddRange((from c in UserRegions.Where(p => p.Deleted == false && p.UserId == userId) join d in CHGSites.Where(p => p.Deleted == false) on c.RegionTypeId equals d.RegionTypeId select d).ToList());
                        break;
                    case "CEO":
                        sites.AddRange((from c in UserCHGSites.Where(p => p.Deleted == false && p.UserId == userId) join d in CHGSites.Where(p => p.Deleted == false) on c.CHGSiteId equals d.CHGSiteId select d).ToList());
                        break;
                    case "DBD":
                        sites.AddRange((from c in UserCHGSites.Where(p => p.Deleted == false && p.UserId == userId) join d in CHGSites.Where(p => p.Deleted == false) on c.CHGSiteId equals d.CHGSiteId select d).ToList());
                        break;
                    case "CL":
                        sites.AddRange((from c in UserCHGSites.Where(p => p.Deleted == false && p.UserId == userId) join d in CHGSites.Where(p => p.Deleted == false) on c.CHGSiteId equals d.CHGSiteId select d).ToList());
                        break;

                }
            }
            return sites.Distinct().ToList();
        }


     
        public void AddPreScreenStatusLog(long preScreenId, string status, long userId)
        {
            PreScreenStatusLog entity = new PreScreenStatusLog();
            entity.UserId = userId;
            entity.Status = status;
            entity.PreScreenId = preScreenId;
            PreScreenStatusLogs.Add(entity);
        }




        #endregion
    }
}
