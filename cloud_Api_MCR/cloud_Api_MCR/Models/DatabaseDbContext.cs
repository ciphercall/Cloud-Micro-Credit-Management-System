using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using cloud_Api_MCR.Models.Account;
using cloud_Api_MCR.Models.ASL;
using cloud_Api_MCR.Models.MCR;

namespace cloud_Api_MCR.Models
{
    public class DatabaseDbContext : DbContext
    {
        //ASL
        public DbSet<AslCompany> AslCompanyDbSet { get; set; }
        public DbSet<AslUserco> AslUsercoDbSet { get; set; }
        public DbSet<AslToken> AslTokenDbSet { get; set; }
        public DbSet<AslMenumst> AslMenumstDbSet { get; set; }
        public DbSet<AslMenu> AslMenuDbSet { get; set; }
        public DbSet<AslRole> AslRoleDbSet { get; set; }
        public DbSet<AslLog> AslLogDbSet { get; set; }
        public DbSet<AslDelete> AslDeleteDbSet { get; set; }



        //Promotionl
        public DbSet<AslPromotionalSms> AslPromotionalSmsDbSet { get; set; }
        public DbSet<AslPromotionalEmail> AslPromotionalEmailDbSet { get; set; }
        public DbSet<AslPromotionalContact> AslPromotionalContactDbSet { get; set; }
        public DbSet<AslPromotionalGroups> AslPromotionalGroupsDbSet { get; set; }



        //GL-Account
        public DbSet<GlAcchartMst> GlAccharMstDbSet { get; set; }
        public DbSet<GlAcchart> GlAcchartDbSet { get; set; }
        public DbSet<GlCostPmst> GlCostPmstDbSet { get; set; }
        public DbSet<GlCostPool> GlCostPoolDbSet { get; set; }
        public DbSet<GlMaster> GlMasterDbSet { get; set; }
        public DbSet<GlStrans> GlStransDbSet { get; set; }


        //MCR (Micro-Credit)
        public DbSet<McrBranch> McrBranchesDbSet { get; set; }
        public DbSet<McrArea> McrAreasDbSet { get; set; }
        public DbSet<McrScheme> McrSchemesDbSet { get; set; }
        public DbSet<McrMember> McrMembersDbSet { get; set; }
        public DbSet<McrMScheme> McrMSchemesDbSet { get; set; }
        public DbSet<McrMNominee> McrMNomineesDbSet { get; set; }
        public DbSet<McrCollect> McrCollectsDbSet { get; set; }
        public DbSet<McrMTrans> McrMTransesDbSet { get; set; }
        public DbSet<McrMLoan> McrMLoansDbSet { get; set; }
        public DbSet<McrMaster> McrMastersDbSet { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

        }
    }
}