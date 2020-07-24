using Microsoft.EntityFrameworkCore;
using Noot.DataAccess.Models;

namespace Noot.DataAccess.Database
{
    public partial class CordisDatabase : DbContext
    {
        public CordisDatabase() : base()
        {
        }

        public CordisDatabase(DbContextOptions<CordisDatabase> Options) : base(Options)
        {
        }

        public DbSet<Appointment> Appointments  { get; set; }
        public DbSet<Assignment> Assignments  { get; set; }
        public DbSet<ControlPoint> ControlPoints  { get; set; }
        public DbSet<ControlPointEAV> ControlPointEAVs  { get; set; }
        public DbSet<ControlPointType> ControlPointTypes  { get; set; }
        public DbSet<DatabaseVersion> DatabaseVersions  { get; set; }
        public DbSet<DatabaseVersionHistory> DatabaseVersionHistories  { get; set; }
        public DbSet<EmployeeImport> EmployeeImports  { get; set; }
        public DbSet<Equipment> Equipment  { get; set; }
        public DbSet<EquipmentEAV> EquipmentEAVs  { get; set; }
        public DbSet<EquipmentLocation> EquipmentLocations  { get; set; }
        public DbSet<EquipmentPosition> EquipmentPositions  { get; set; }
        public DbSet<EquipmentSchedule> EquipmentSchedules  { get; set; }
        public DbSet<EquipmentStatus> EquipmentStatuses  { get; set; }
        public DbSet<EquipmentType> EquipmentTypes  { get; set; }
        public DbSet<EquipmentTypeEAV> EquipmentTypeEAVs  { get; set; }
        public DbSet<Event> Events  { get; set; }
        public DbSet<EventCategory> EventCategories  { get; set; }
        public DbSet<EventLog> EventLogs  { get; set; }
        public DbSet<EventRecurrence> EventRecurrences  { get; set; }
        public DbSet<IdentifierType> IdentifierTypes  { get; set; }
        public DbSet<ImportSettings> ImportSettings  { get; set; }
        public DbSet<Location> Locations  { get; set; }
        public DbSet<LocationEAV> LocationEAVs  { get; set; }
        public DbSet<LocationGraphicReference> LocationGraphicReferences  { get; set; }
        public DbSet<LocationPosition> LocationPositions  { get; set; }
        public DbSet<LocationType> LocationTypes  { get; set; }
        public DbSet<LocationTypeEAV> LocationTypeEAVs  { get; set; }
        public DbSet<LocationTypeEntity> LocationTypeEntities  { get; set; }
        public DbSet<LocationTypeGroup> LocationTypeGroups  { get; set; }
        public DbSet<MapSet> MapSets  { get; set; }
        public DbSet<MapSetQuery> MapSetQueries  { get; set; }
        public DbSet<MapSetTarget> MapSetTargets  { get; set; }
        public DbSet<MapSetValue> MapSetValues  { get; set; }
        public DbSet<Material> Materials  { get; set; }
        public DbSet<MaterialEAV> MaterialEAVs  { get; set; }
        public DbSet<MaterialType> MaterialTypes  { get; set; }
        public DbSet<NamingStandard> NamingStandards  { get; set; }
        public DbSet<ObjectEntity> ObjectEntities  { get; set; }
        public DbSet<Occupation> Occupations  { get; set; }
        public DbSet<OccupationEAV> OccupationEAVs  { get; set; }
        public DbSet<OrganizationPosition> OrganizationPositions  { get; set; }
        public DbSet<OrganizationStructure> OrganizationStructures  { get; set; }
        public DbSet<OrganizationStructureEAV> OrganizationStructureEAVs  { get; set; }
        public DbSet<Person> People  { get; set; }
        public DbSet<PersonEAV> PersonEAVs  { get; set; }
        public DbSet<PersonIdentifier> PersonIdentifiers  { get; set; }
        public DbSet<PositionType> PositionTypes  { get; set; }
        public DbSet<PositionTypeBorder> PositionTypeBorders  { get; set; }
        public DbSet<PositionTypeEAV> PositionTypeEAVs  { get; set; }
        public DbSet<PositionTypeOccupation> PositionTypeOccupations  { get; set; }
        public DbSet<PositionTypeShape> PositionTypeShapes  { get; set; }
        public DbSet<Schedule> Schedules  { get; set; }
        public DbSet<SchedulePosition> SchedulePositions  { get; set; }
        public DbSet<SchemaVersion> SchemaVersions  { get; set; }
        public DbSet<SchemaVersionHistory> SchemaVersionHistories  { get; set; }
        public DbSet<Site> Sites  { get; set; }
        public DbSet<SiteEAV> SiteEAVs  { get; set; }
        public DbSet<SiteSecurity> SiteSecurities  { get; set; }
        public DbSet<StageOrg> StageOrgs  { get; set; }
        public DbSet<StageOrgOne> StageOrgOnes  { get; set; }
        public DbSet<StageOrgTree> StageOrgTrees  { get; set; }
        public DbSet<StageOrgTwo> StageOrgTwos  { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder OptionsBuilder)
        {
            OptionsBuilder.UseSqlServer(@"Data Source=geoinventory.southafricanorth.cloudapp.azure.com;Initial Catalog=CORDISDB;User ID=InventAdmin;Password=Geonator1234;Connect Timeout=30");
        }

        protected override void OnModelCreating(ModelBuilder ModelBuilder)
        {
            ModelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasIndex(e => new { e.OccupationID, e.PersonID, e.SiteID, e.StartDate })
                    .HasName("AK_Appointment")
                    .IsUnique();

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.PersonID)
                    .HasConstraintName("FK_PersonAppointment");
                entity.HasOne(d => d.Occupation)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.OccupationID)
                    .HasConstraintName("FK_OccupationAppointment");
                entity.HasOne(d => d.Site)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.SiteID)
                    .HasConstraintName("FK_SitePersonAppointment");

            });

            ModelBuilder.Entity<Assignment>(entity =>
            {
                entity.HasIndex(e => new { e.OrganizationPositionID, e.PersonID, e.StartDate })
                    .HasName("AK_Assignment")
                    .IsUnique();

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Assignments)
                    .HasForeignKey(d => d.PersonID)
                    .HasConstraintName("FK_Person_Assignment");
                entity.HasOne(d => d.OrganizationPosition)
                    .WithMany(p => p.Assignments)
                    .HasForeignKey(d => d.OrganizationPositionID)
                    .HasConstraintName("FK_OrganizationPosition_Assignment");

            });

            ModelBuilder.Entity<ControlPoint>(entity =>
            {
                entity.HasIndex(e => new { e.ControlPointName, e.GroupID })
                    .HasName("IX_ControlPoints")
                    .IsUnique();

                entity.HasOne(d => d.BacksightControlPoint)
                    .WithMany(p => p.ControlPoints)
                    .HasForeignKey(d => d.BacksightControlPointID)
                    .HasConstraintName("FK_ControlPoint_ControlPoint");
                entity.HasOne(d => d.ControlPointType)
                    .WithMany(p => p.ControlPoints)
                    .HasForeignKey(d => d.ControlPointTypeID)
                    .HasConstraintName("FK_ControlPoint_ControlPointType");

            });

            ModelBuilder.Entity<ControlPointEAV>(entity =>
            {
                entity.HasIndex(e => new { e.ElementID, e.AttributeID, e.ValueNo })
                    .HasName("AK_ControlPointEAV")
                    .IsUnique();

                entity.HasOne(d => d.Element)
                    .WithMany(p => p.ControlPointEAVs)
                    .HasForeignKey(d => d.ElementID)
                    .HasConstraintName("FK_ControlPointEAV_ControlPoint");

            });

            ModelBuilder.Entity<ControlPointType>(entity =>
            {
                entity.HasIndex(e => new { e.Name })
                    .HasName("AK_ControlPointType_Name")
                    .IsUnique();

            });

            ModelBuilder.Entity<DatabaseVersion>(entity =>
            {
            });

            ModelBuilder.Entity<DatabaseVersionHistory>(entity =>
            {
            });

            ModelBuilder.Entity<EmployeeImport>(entity =>
            {
                entity.HasNoKey();
            });


            ModelBuilder.Entity<Equipment>(entity =>
            {
                entity.HasIndex(e => new { e.Name, e.EquipmentTypeID, e.SiteID })
                    .HasName("AK_Equipment")
                    .IsUnique();

                entity.HasOne(d => d.EquipmentStatus)
                    .WithMany(p => p.Equipment)
                    .HasForeignKey(d => d.EquipmentStatusID)
                    .HasConstraintName("FK_EquipmentStatus_Equipment");
                entity.HasOne(d => d.EquipmentType)
                    .WithMany(p => p.Equipment)
                    .HasForeignKey(d => d.EquipmentTypeID)
                    .HasConstraintName("FK_EquipmentType_Equipment");
                entity.HasOne(d => d.Site)
                    .WithMany(p => p.Equipment)
                    .HasForeignKey(d => d.SiteID)
                    .HasConstraintName("FK_Site_Equipment");

            });

            ModelBuilder.Entity<EquipmentEAV>(entity =>
            {
                entity.HasIndex(e => new { e.ElementID, e.AttributeID, e.ValueNo })
                    .HasName("AK_EquipmentEAV")
                    .IsUnique();

                entity.HasOne(d => d.Element)
                    .WithMany(p => p.EquipmentEAVs)
                    .HasForeignKey(d => d.ElementID)
                    .HasConstraintName("FK_Equipment_EquipmentEAV");

            });

            ModelBuilder.Entity<EquipmentLocation>(entity =>
            {
                entity.HasIndex(e => new { e.EquipmentID, e.LocationID, e.StartDate })
                    .HasName("AK_EquipmentLocation")
                    .IsUnique();

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.EquipmentLocations)
                    .HasForeignKey(d => d.EquipmentID)
                    .HasConstraintName("FK_Equipment_EquipmentLocation");
                entity.HasOne(d => d.Location)
                    .WithMany(p => p.EquipmentLocations)
                    .HasForeignKey(d => d.LocationID)
                    .HasConstraintName("FK_Location_EquipmentLocation");

            });

            ModelBuilder.Entity<EquipmentPosition>(entity =>
            {
                entity.HasIndex(e => new { e.EquipmentID, e.PositionID, e.StartDate })
                    .HasName("AK_EquipmentPosition")
                    .IsUnique();

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.EquipmentPositions)
                    .HasForeignKey(d => d.PositionID)
                    .HasConstraintName("FK_OrganizationPosition_EquipmentPosition");
                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.EquipmentPositions)
                    .HasForeignKey(d => d.EquipmentID)
                    .HasConstraintName("FK_Equipment_EquipmentPosition");

            });

            ModelBuilder.Entity<EquipmentSchedule>(entity =>
            {
                entity.HasOne(d => d.Schedule)
                    .WithMany(p => p.EquipmentSchedules)
                    .HasForeignKey(d => d.ScheduleID)
                    .HasConstraintName("FK_Schedule_EquipmentSchedule");
                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.EquipmentSchedules)
                    .HasForeignKey(d => d.EquipmentID)
                    .HasConstraintName("FK_Equipment_EquipmentSchedule");

            });

            ModelBuilder.Entity<EquipmentStatus>(entity =>
            {
                entity.HasIndex(e => new { e.Name })
                    .HasName("AK_EquipmentStatus")
                    .IsUnique();

            });

            ModelBuilder.Entity<EquipmentType>(entity =>
            {
                entity.HasIndex(e => new { e.Name })
                    .HasName("AK_EquipmentType")
                    .IsUnique();

            });

            ModelBuilder.Entity<EquipmentTypeEAV>(entity =>
            {
                entity.HasIndex(e => new { e.ElementID, e.AttributeID, e.ValueNo })
                    .HasName("AK_EquipmentTypeEAV")
                    .IsUnique();

                entity.HasOne(d => d.Element)
                    .WithMany(p => p.EquipmentTypeEAVs)
                    .HasForeignKey(d => d.ElementID)
                    .HasConstraintName("FK_EquipmentType_EquipmentTypeEAV");

            });

            ModelBuilder.Entity<Event>(entity =>
            {
                entity.HasOne(d => d.EventCategory)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.EventCategoryID)
                    .HasConstraintName("FK_Event_EventCategory");
                entity.HasOne(d => d.Schedule)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.ScheduleID)
                    .HasConstraintName("FK_Event_Schedule");
                entity.HasOne(d => d.ExceptionForEvent)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.ExceptionForEventID)
                    .HasConstraintName("FK_Event_ExceptionEvent");

            });

            ModelBuilder.Entity<EventCategory>(entity =>
            {
                entity.HasIndex(e => new { e.Name, e.SiteID })
                    .HasName("AK_EventCategory")
                    .IsUnique();

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.EventCategories)
                    .HasForeignKey(d => d.SiteID)
                    .HasConstraintName("FK_Site_EventCategory");

            });

            ModelBuilder.Entity<EventLog>(entity =>
            {
            });

            ModelBuilder.Entity<EventRecurrence>(entity =>
            {
                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventRecurrences)
                    .HasForeignKey(d => d.EventID)
                    .HasConstraintName("FK_EventRecurrence_Event");

            });

            ModelBuilder.Entity<IdentifierType>(entity =>
            {
                entity.HasIndex(e => new { e.Name })
                    .HasName("AK_IdentifierType")
                    .IsUnique();

            });

            ModelBuilder.Entity<ImportSettings>(entity =>
            {
            });

            ModelBuilder.Entity<Location>(entity =>
            {
                entity.HasIndex(e => new { e.Name, e.LocationTypeID, e.SiteID })
                    .HasName("AK_Location")
                    .IsUnique();

                entity.HasOne(d => d.LocationType)
                    .WithMany(p => p.Locations)
                    .HasForeignKey(d => d.LocationTypeID)
                    .HasConstraintName("FK_LocationType_Location");
                entity.HasOne(d => d.Site)
                    .WithMany(p => p.Locations)
                    .HasForeignKey(d => d.SiteID)
                    .HasConstraintName("FK_Site_Location");
                entity.HasOne(d => d.GreaterLocation)
                    .WithMany(p => p.Locations)
                    .HasForeignKey(d => d.GreaterLocationID)
                    .HasConstraintName("FK_Location_Location");

            });

            ModelBuilder.Entity<LocationEAV>(entity =>
            {
                entity.HasIndex(e => new { e.ElementID, e.AttributeID, e.ValueNo })
                    .HasName("AK_LocationEAV")
                    .IsUnique();

                entity.HasOne(d => d.Element)
                    .WithMany(p => p.LocationEAVs)
                    .HasForeignKey(d => d.ElementID)
                    .HasConstraintName("FK_LocationEAV_Location");

            });

            ModelBuilder.Entity<LocationGraphicReference>(entity =>
            {
                entity.HasIndex(e => new { e.LocationID, e.LibraryID, e.ElementID })
                    .HasName("AK_LocationGraphicReference")
                    .IsUnique();

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.LocationGraphicReferences)
                    .HasForeignKey(d => d.LocationID)
                    .HasConstraintName("FK_Location_LocationGraphicReference");

            });

            ModelBuilder.Entity<LocationPosition>(entity =>
            {
                entity.HasIndex(e => new { e.LocationID, e.OrganizationPositionID, e.AssignmentStartDate })
                    .HasName("AK_OrganisationPositionLocation")
                    .IsUnique();

                entity.HasOne(d => d.OrganizationPosition)
                    .WithMany(p => p.LocationPositions)
                    .HasForeignKey(d => d.OrganizationPositionID)
                    .HasConstraintName("FK_OrganizationPosition_LocationPosition");
                entity.HasOne(d => d.Location)
                    .WithMany(p => p.LocationPositions)
                    .HasForeignKey(d => d.LocationID)
                    .HasConstraintName("FK_Location_LocationPosition");

            });

            ModelBuilder.Entity<LocationType>(entity =>
            {
                entity.HasIndex(e => new { e.Name })
                    .HasName("AK_LocationType")
                    .IsUnique();

                entity.HasOne(d => d.LocationTypeGroup)
                    .WithMany(p => p.LocationTypes)
                    .HasForeignKey(d => d.LocationTypeGroupID)
                    .HasConstraintName("FK_LocationType_LocationTypeGroup");
                entity.HasOne(d => d.NamingStandard)
                    .WithMany(p => p.LocationTypes)
                    .HasForeignKey(d => d.DisplayNamingStandardID)
                    .HasConstraintName("FK_NamingStandard_LocationType_DisplayNamingStandardID");
                entity.HasOne(d => d.NamingStandard)
                    .WithMany(p => p.LocationTypes)
                    .HasForeignKey(d => d.NamingStandardID)
                    .HasConstraintName("FK_NamingStandard_LocationType_NamingStandardID");

            });

            ModelBuilder.Entity<LocationTypeEAV>(entity =>
            {
                entity.HasIndex(e => new { e.ElementID, e.AttributeID, e.ValueNo })
                    .HasName("AK_LocationTypeEAV")
                    .IsUnique();

                entity.HasOne(d => d.Element)
                    .WithMany(p => p.LocationTypeEAVs)
                    .HasForeignKey(d => d.ElementID)
                    .HasConstraintName("FK_LocationType_LocationTypeEAV");

            });

            ModelBuilder.Entity<LocationTypeEntity>(entity =>
            {
                entity.HasIndex(e => new { e.LocationTypeID, e.EntityID })
                    .HasName("AK_LocationTypeEntity")
                    .IsUnique();

                entity.HasOne(d => d.LocationType)
                    .WithMany(p => p.LocationTypeEntities)
                    .HasForeignKey(d => d.LocationTypeID)
                    .HasConstraintName("FK_LocationType_LocationTypeEntity");

            });

            ModelBuilder.Entity<LocationTypeGroup>(entity =>
            {
                entity.HasIndex(e => new { e.Name, e.SiteID })
                    .HasName("AK_LocationTypeGroup")
                    .IsUnique();

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.LocationTypeGroups)
                    .HasForeignKey(d => d.SiteID)
                    .HasConstraintName("FK_Site_LocationTypeGroup");

            });

            ModelBuilder.Entity<MapSet>(entity =>
            {
                entity.HasIndex(e => new { e.Name })
                    .HasName("AK_MapSet_Name")
                    .IsUnique();

                entity.HasOne(d => d.MasterQuery)
                    .WithMany(p => p.MapSets)
                    .HasForeignKey(d => d.MasterQueryID)
                    .HasConstraintName("FK_MapSet_Query");

            });

            ModelBuilder.Entity<MapSetQuery>(entity =>
            {
                entity.HasIndex(e => new { e.Name })
                    .HasName("AK_MapSetQuery_Name")
                    .IsUnique();

            });

            ModelBuilder.Entity<MapSetTarget>(entity =>
            {
                entity.HasIndex(e => new { e.MapSetID, e.TargetQueryID })
                    .HasName("AK_MapSetTarget_MapSetID_TargetQueryID")
                    .IsUnique();

                entity.HasOne(d => d.MapSet)
                    .WithMany(p => p.MapSetTargets)
                    .HasForeignKey(d => d.MapSetID)
                    .HasConstraintName("FK_MapSetTarget_MapSet");
                entity.HasOne(d => d.TargetQuery)
                    .WithMany(p => p.MapSetTargets)
                    .HasForeignKey(d => d.TargetQueryID)
                    .HasConstraintName("FK_MapSetTarget_Query");

            });

            ModelBuilder.Entity<MapSetValue>(entity =>
            {
                entity.HasIndex(e => new { e.MapSetID, e.MapSetTargetID, e.MasterValue })
                    .HasName("AK_MapSetValue_MapSetID_MapSetTargetID")
                    .IsUnique();

                entity.HasOne(d => d.MapSet)
                    .WithMany(p => p.MapSetValues)
                    .HasForeignKey(d => d.MapSetID)
                    .HasConstraintName("FK_Map_MapSet");
                entity.HasOne(d => d.MapSetTarget)
                    .WithMany(p => p.MapSetValues)
                    .HasForeignKey(d => d.MapSetTargetID)
                    .HasConstraintName("FK_Map_MapSetTarget");

            });

            ModelBuilder.Entity<Material>(entity =>
            {
                entity.HasIndex(e => new { e.Name, e.MaterialTypeID, e.SiteID })
                    .HasName("AK_Material_Name")
                    .IsUnique();

                entity.HasOne(d => d.MaterialType)
                    .WithMany(p => p.Materials)
                    .HasForeignKey(d => d.MaterialTypeID)
                    .HasConstraintName("FK_MaterialType_Material");
                entity.HasOne(d => d.Site)
                    .WithMany(p => p.Materials)
                    .HasForeignKey(d => d.SiteID)
                    .HasConstraintName("FK_Site_Material");

            });

            ModelBuilder.Entity<MaterialEAV>(entity =>
            {
                entity.HasIndex(e => new { e.ElementID, e.AttributeID, e.ValueNo })
                    .HasName("AK_MaterialEAV")
                    .IsUnique();

                entity.HasOne(d => d.Element)
                    .WithMany(p => p.MaterialEAVs)
                    .HasForeignKey(d => d.ElementID)
                    .HasConstraintName("FK_Material_MaterialEAV");

            });

            ModelBuilder.Entity<MaterialType>(entity =>
            {
                entity.HasIndex(e => new { e.Name })
                    .HasName("AK_MaterialType_Name")
                    .IsUnique();

            });

            ModelBuilder.Entity<NamingStandard>(entity =>
            {
                entity.HasIndex(e => new { e.Name })
                    .HasName("AK_NamingStandard")
                    .IsUnique();

            });

            ModelBuilder.Entity<ObjectEntity>(entity =>
            {
            });

            ModelBuilder.Entity<Occupation>(entity =>
            {
                entity.HasIndex(e => new { e.Name })
                    .HasName("AK_Occupation")
                    .IsUnique();

            });

            ModelBuilder.Entity<OccupationEAV>(entity =>
            {
                entity.HasIndex(e => new { e.ElementID, e.AttributeID, e.ValueNo })
                    .HasName("AK_OccupationEAV")
                    .IsUnique();

                entity.HasOne(d => d.Element)
                    .WithMany(p => p.OccupationEAVs)
                    .HasForeignKey(d => d.ElementID)
                    .HasConstraintName("FK_Occupation_OccupationEAV");

            });

            ModelBuilder.Entity<OrganizationPosition>(entity =>
            {
                entity.HasIndex(e => new { e.Name, e.OrganizationStructureID, e.StartDate })
                    .HasName("AK_OrganizationPosition")
                    .IsUnique();
                entity.HasIndex(e => new { e.OrganizationPositionID })
                    .HasName("IX_OrganizationPosition");

                entity.HasOne(d => d.OrganizationStructure)
                    .WithMany(p => p.OrganizationPositions)
                    .HasForeignKey(d => d.OrganizationStructureID)
                    .HasConstraintName("FK_OrganizationStructure_OrganizationPosition");
                entity.HasOne(d => d.PositionType)
                    .WithMany(p => p.OrganizationPositions)
                    .HasForeignKey(d => d.PositionTypeID)
                    .HasConstraintName("FK_PositionType_OrganizationPosition");
                entity.HasOne(d => d.SuperiorPosition)
                    .WithMany(p => p.OrganizationPositions)
                    .HasForeignKey(d => d.SuperiorPositionID)
                    .HasConstraintName("FK_OrganizationPosition_OrganizationPosition");

            });

            ModelBuilder.Entity<OrganizationStructure>(entity =>
            {
                entity.HasIndex(e => new { e.Name })
                    .HasName("AK_OrganizationStructure")
                    .IsUnique();

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.OrganizationStructures)
                    .HasForeignKey(d => d.SiteID)
                    .HasConstraintName("FK_Site_OrganizationStructure");

            });

            ModelBuilder.Entity<OrganizationStructureEAV>(entity =>
            {
                entity.HasIndex(e => new { e.ElementID, e.AttributeID, e.ValueNo })
                    .HasName("AK_OrganizationStructureEAV")
                    .IsUnique();

                entity.HasOne(d => d.Element)
                    .WithMany(p => p.OrganizationStructureEAVs)
                    .HasForeignKey(d => d.ElementID)
                    .HasConstraintName("FK_OrganizationStructure_OrganizationStructureEAV");

            });

            ModelBuilder.Entity<Person>(entity =>
            {
                entity.HasIndex(e => new { e.MineFormsUserID })
                    .HasName("IX_MineFormsUserID")
                    .IsUnique();
                entity.HasIndex(e => new { e.SecUserID })
                    .HasName("IX_SecUserID")
                    .IsUnique();

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.People)
                    .HasForeignKey(d => d.SiteID)
                    .HasConstraintName("FK_Site_Person");

            });

            ModelBuilder.Entity<PersonEAV>(entity =>
            {
                entity.HasIndex(e => new { e.ElementID, e.AttributeID, e.ValueNo })
                    .HasName("AK_PersonEAV")
                    .IsUnique();

                entity.HasOne(d => d.Element)
                    .WithMany(p => p.PersonEAVs)
                    .HasForeignKey(d => d.ElementID)
                    .HasConstraintName("FK_PersonEAV_Person");

            });

            ModelBuilder.Entity<PersonIdentifier>(entity =>
            {
                entity.HasIndex(e => new { e.IdentifierTypeID, e.PersonID })
                    .HasName("AK_PersonIdentifier")
                    .IsUnique();
                entity.HasIndex(e => new { e.IdentifierTypeID, e.NumericValue })
                    .HasName("IX_NumericValue")
                    .IsUnique();
                entity.HasIndex(e => new { e.IdentifierTypeID, e.TextValue })
                    .HasName("IX_TextValue")
                    .IsUnique();

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.PersonIdentifiers)
                    .HasForeignKey(d => d.PersonID)
                    .HasConstraintName("FK_Person_PersonIdentifier");
                entity.HasOne(d => d.IdentifierType)
                    .WithMany(p => p.PersonIdentifiers)
                    .HasForeignKey(d => d.IdentifierTypeID)
                    .HasConstraintName("FK_IdentifierType_PersonIdentifier");

            });

            ModelBuilder.Entity<PositionType>(entity =>
            {
                entity.HasIndex(e => new { e.SiteID, e.Name })
                    .HasName("AK_PositionType")
                    .IsUnique();

                entity.HasOne(d => d.Border)
                    .WithMany(p => p.PositionTypes)
                    .HasForeignKey(d => d.BorderID)
                    .HasConstraintName("FK_PositionType_PositionTypeBorder");
                entity.HasOne(d => d.Shape)
                    .WithMany(p => p.PositionTypes)
                    .HasForeignKey(d => d.ShapeID)
                    .HasConstraintName("FK_PositionTypeShape_PositionType");
                entity.HasOne(d => d.Site)
                    .WithMany(p => p.PositionTypes)
                    .HasForeignKey(d => d.SiteID)
                    .HasConstraintName("FK_Site_PositionType");

            });

            ModelBuilder.Entity<PositionTypeBorder>(entity =>
            {
            });

            ModelBuilder.Entity<PositionTypeEAV>(entity =>
            {
                entity.HasIndex(e => new { e.ElementID, e.AttributeID, e.ValueNo })
                    .HasName("AK_PositionTypeEAV")
                    .IsUnique();

                entity.HasOne(d => d.Element)
                    .WithMany(p => p.PositionTypeEAVs)
                    .HasForeignKey(d => d.ElementID)
                    .HasConstraintName("FK_PositionType_PositionTypeEAV");

            });

            ModelBuilder.Entity<PositionTypeOccupation>(entity =>
            {
                entity.HasIndex(e => new { e.PositionTypeID, e.OccupationID })
                    .HasName("AK_PositionTypeOccupation")
                    .IsUnique();

                entity.HasOne(d => d.PositionType)
                    .WithMany(p => p.PositionTypeOccupations)
                    .HasForeignKey(d => d.PositionTypeID)
                    .HasConstraintName("FK_PositionType_PositionTypeOccupation");
                entity.HasOne(d => d.Occupation)
                    .WithMany(p => p.PositionTypeOccupations)
                    .HasForeignKey(d => d.OccupationID)
                    .HasConstraintName("FK_Occupation_PositionTypeOccupation");

            });

            ModelBuilder.Entity<PositionTypeShape>(entity =>
            {
                entity.HasIndex(e => new { e.Name })
                    .HasName("AK_PositionTypeShape")
                    .IsUnique();

            });

            ModelBuilder.Entity<Schedule>(entity =>
            {
                entity.HasOne(d => d.Site)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.SiteID)
                    .HasConstraintName("FK_Schedule_Site");

            });

            ModelBuilder.Entity<SchedulePosition>(entity =>
            {
                entity.HasIndex(e => new { e.ScheduleID, e.OrganizationPositionID })
                    .HasName("AK_SchedulePosition")
                    .IsUnique();

                entity.HasOne(d => d.Schedule)
                    .WithMany(p => p.SchedulePositions)
                    .HasForeignKey(d => d.ScheduleID)
                    .HasConstraintName("FK_Schedule_SchedulePosition");
                entity.HasOne(d => d.OrganizationPosition)
                    .WithMany(p => p.SchedulePositions)
                    .HasForeignKey(d => d.OrganizationPositionID)
                    .HasConstraintName("FK_OrganizationPosition_SchedulePosition");

            });

            ModelBuilder.Entity<SchemaVersion>(entity =>
            {
            });

            ModelBuilder.Entity<SchemaVersionHistory>(entity =>
            {
            });

            ModelBuilder.Entity<Site>(entity =>
            {
                entity.HasIndex(e => new { e.Name })
                    .HasName("AK_Site_Name")
                    .IsUnique();
                entity.HasIndex(e => new { e.Code })
                    .HasName("IX_Site_Code")
                    .IsUnique();

                entity.HasOne(d => d.SuperiorSite)
                    .WithMany(p => p.Sites)
                    .HasForeignKey(d => d.SuperiorSiteID)
                    .HasConstraintName("FK_Site_Site");

            });

            ModelBuilder.Entity<SiteEAV>(entity =>
            {
                entity.HasIndex(e => new { e.ElementID, e.AttributeID, e.ValueNo })
                    .HasName("AK_SiteEAV")
                    .IsUnique();

                entity.HasOne(d => d.Element)
                    .WithMany(p => p.SiteEAVs)
                    .HasForeignKey(d => d.ElementID)
                    .HasConstraintName("FK_SiteEAV_Site");

            });

            ModelBuilder.Entity<SiteSecurity>(entity =>
            {
                entity.HasOne(d => d.Site)
                    .WithMany(p => p.SiteSecurities)
                    .HasForeignKey(d => d.SiteID)
                    .HasConstraintName("FK_Site_SiteSecurity");

            });

            ModelBuilder.Entity<StageOrg>(entity =>
            {
                entity.HasIndex(e => new { e.ExternalPositionID, e.StartDate })
                    .HasName("StageOrgPK")
                    .IsUnique();

            });

            ModelBuilder.Entity<StageOrgOne>(entity =>
            {
                entity.HasIndex(e => new { e.ExternalPositionID, e.StartDate })
                    .HasName("StageOrgOnePK")
                    .IsUnique();

            });

            ModelBuilder.Entity<StageOrgTree>(entity =>
            {
                entity.HasIndex(e => new { e.ExternalPositionID, e.StartDate })
                    .HasName("StageOrgTreePK")
                    .IsUnique();

            });

            ModelBuilder.Entity<StageOrgTwo>(entity =>
            {
                entity.HasIndex(e => new { e.ExternalPositionID, e.StartDate })
                    .HasName("StageOrgTwoPK")
                    .IsUnique();

            });

        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

