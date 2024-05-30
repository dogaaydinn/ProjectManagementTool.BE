using Core.Context.EntityFramework;
using Domain.Entities.Association;
using Domain.Entities.Communication;
using Domain.Entities.DutyManagement;
using Domain.Entities.DutyManagement.UserManagement;
using Domain.Entities.ProjectManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DataAccess.Context.EntityFramework;

public sealed class EfDbContext : EfDbContextBase
{
    public EfDbContext()
    {
        ChangeTracker.LazyLoadingEnabled = false;
    }

    public DbSet<Project> Projects { get; set; }

    public DbSet<Team> Teams { get; set; }

    public DbSet<TeamProject> TeamProjects { get; set; }
    public DbSet<Duty> Duties { get; set; }

    public DbSet<User> Users { get; set; }
    public DbSet<UserDuty> UserDuties { get; set; }
    public DbSet<UserTeam> UserTeams { get; set; }

    public DbSet<Comment> Comments { get; set; }

    public DbSet<Label> Labels { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // Configure the relationship ParentDuty and SubDuties
    modelBuilder.Entity<Duty>()
        .HasMany(d => d.SubDuties)
        .WithOne(d => d.ParentDuty)
        .HasForeignKey(d => d.ParentDutyId)
        .OnDelete(DeleteBehavior.Restrict);

    // Configure the relationship between Duty and Project
    modelBuilder.Entity<Duty>()
        .HasOne(d => d.Project)
        .WithMany(p => p.Duties)
        .HasForeignKey(d => d.ProjectId)
        .OnDelete(DeleteBehavior.Restrict);

    // Configure the relationship between Duty and Comment
    modelBuilder.Entity<Duty>()
        .HasMany(d => d.Comments)
        .WithOne(c => c.Duty)
        .HasForeignKey(c => c.DutyId)
        .OnDelete(DeleteBehavior.Restrict);

    // Configure the relationship between Duty and Label
    modelBuilder.Entity<Duty>()
        .HasMany(d => d.Labels)
        .WithMany(l => l.Duties)
        .UsingEntity<Dictionary<string, object>>(
            "DutyLabel",
            j => j
                .HasOne<Label>()
                .WithMany()
                .HasForeignKey("LabelId"),
            j => j
                .HasOne<Duty>()
                .WithMany()
                .HasForeignKey("DutyId")
        );

    // Configure the relationship between Duty and User (AssignedUsers)
    modelBuilder.Entity<Duty>()
        .HasMany(d => d.AssignedUsers)
        .WithMany(u => u.AssignedDuties)
        .UsingEntity<Dictionary<string, object>>(
            "DutyUser",
            j => j
                .HasOne<User>()
                .WithMany()
                .HasForeignKey("UserId"),
            j => j
                .HasOne<Duty>()
                .WithMany()
                .HasForeignKey("DutyId")
        );

    // Configure the relationship between User and Duty (UserDuties)
    modelBuilder.Entity<User>()
        .HasMany(u => u.AssignedDuties)
        .WithMany(d => d.AssignedUsers)
        .UsingEntity<Dictionary<string, object>>(
            "DutyUser",
            j => j
                .HasOne<Duty>()
                .WithMany()
                .HasForeignKey("DutyId"),
            j => j
                .HasOne<User>()
                .WithMany()
                .HasForeignKey("UserId")
        );

    // Configure the relationship between User and ManagedTeams
    modelBuilder.Entity<User>()
        .HasMany(u => u.ManagedTeams)
        .WithOne(t => t.Manager)
        .HasForeignKey(t => t.ManagerId)
        .OnDelete(DeleteBehavior.Restrict);

    // Configure the relationship between User and ReportedDuties
    modelBuilder.Entity<User>()
        .HasMany(u => u.ReportedDuties)
        .WithOne(d => d.Reporter)
        .HasForeignKey(d => d.ReporterId)
        .OnDelete(DeleteBehavior.Restrict);

    // Configure the relationship between Project and User (Manager)
    modelBuilder.Entity<Project>()
        .HasOne(p => p.Manager)
        .WithMany(u => u.ManagedProjects)
        .HasForeignKey(p => p.ManagerId)
        .OnDelete(DeleteBehavior.Cascade);
    
    modelBuilder.Entity<User>()
        .Property(u => u.FirstName)
        .IsRequired();

}
    

    #region büyükbirricın

    #region EF Core'da Neden Yapılandırmalara İhtiyacımız Olur?

//Default davranışları yeri geldiğinde geçersiz kılmak ve özelleştirmek isteyebiliriz. Bundan dolayı yapılandırmalara ihtiyacımız olacaktır.

    #endregion

    #region OnModelCreating Metodu

//EF Core'da yapılandırma deyince akla ilk gelen metot OnModelCreating metodudur.
//Bu metot, DbContext sınıfı içerisinde virtual olarak ayarlanmış bir metottur.
//Bizler bu metodu kullanarak model'larımızla ilgili temel konfigürasyonel davranışları(Fluent API) sergileyeibliriz.
//Bir model'ın yaratılışıyla ilgili tüm konfigürasyonları burada gerçekleştirebilmekteyiz.

    #endregion

    #region Configurations | Data Annotations & Fluent API

    #region Table - ToTable

//Generate edilecek tablonun ismini belirlememizi sağlayan yapılandırmadır.
//Ef Core normal şartlarda generate edeceği tablonun adını DbSet property'sinden almaktadır. Bizler eğer ki bunu özelleştirmek istiyorsak Table attribute'unu yahut ToTable api'ını kullanabilriiz.

    #endregion

    #region Column - HasColumnName, HasColumnType, HasColumnOrder

//EF Core'da tabloların kolonları entity sınıfları içerisindeki property'lere karşılık gelmektedir. 
//Default olarak property'lerin adı kolon adıyken, türleri/tipleri kolon türleridir.
//Eğer ki generate edilecek kolon isimlerine ve türlerine müdahale etmek sitiyorsak bu konfigürasyon kullanılır.

    #endregion

    #region ForeignKey - HasForeignKey

//İlişkisel tablo tasarımlarında, bağımlı tabloda esas tabloya karşılık gelecek verilerin tutulduğu kolonu foreign key olarak temsil etmekteyiz.
//EF Core'da foreign key kolonu genellikle Entity Tanımlama kuralları gereği default yapılanmalarla oluşturulur.
//ForeignKey Data Annotations Attribute'unu direkt kullanabilirsiniz. Lakin Fluent api ile bu konfigürasyonu yapacaksanız iki entity arasındaki ilişkiyide modellemeniz gerekmektedir. Aksi taktirde fluent api üzerinde HasForeignKey fonksiyonunu kullanamazsınız!

    #endregion

    #region NotMapped - Ignore

//EF Core, entity sınıfları içerisindeki tüm proeprtyleri default olarak modellenen tabloya kolon şeklinde migrate eder.
//Bazn bizler entity sınıfları içerisinde tabloda bir kolona karşılık gelmeyen propertyler tanımlamak mecburiyetinde kalabiliriz.
//Bu property'lerin ef core tarafından kolon olarak map edilmesini istemediğimizi bildirebilmek için NotMapped ya da Ignore kullanabiliriz.

    #endregion

    #region Key - HasKey

//EF Core'da, default convention olarak bir entity'nin içerisinde Id, ID, EntityId, EntityID vs. şeklinde tanımlanan tüm propertylere varsayılan olarak primary key constraint uygulanır.
//Key ya da HasKey yapılanmalarıyla istediğinmiz her hangi bir property'e default convention dışında pk uygulayabiliriz.
//EF Core'da bir entity içerisinde kesinlikle PK'i temsil edecek olan property bulunmalıdır. Aksi taktirde EF Core migration oluşurken hata verecektir. Eğer ki tablonun PK'i yoksa bunun bildirilmesi gerekir. 

    #endregion

    #region Timestamp - IsRowVersion

// Bir verinin versiyonunu oluşturmamızı sağlayan yapılanma bu konfigürasyonlardır.

    #endregion

    #region Required - IsRequired

//Bir kolonun nullable ya da not null olup olmamasını bu konfigürasyonla belirleyebiliriz.
//EF Core'da bir property default olarak not null şeklinde tanımlanır. Eğer ki property'si nullable yapmak istyorsak türü üzerinde ?(nullable) operatörü ile bbildirimde bulunmamız gerekmektedir.

    #endregion

    #region MaxLenght | StringLength - HasMaxLength

//Bir kolonun max karakter sayısını belirlememizi sağlar.

    #endregion

    #region Precision - HasPrecision

//Küsüratlı sayılarda bir kesinlik belirtmemizi ve noktanın hanesini bildirmemizi sağlayan bir yapılandırmadır.

    #endregion

    #region Unicode - IsUnicode

//Kolon içerisinde unicode karakterler kullanılacaksa bu yapılandırmadan istifade edilebilir.

    #endregion

    #region Comment - HasComment

//EF Core üzerinden oluşturulmuş olan veritabanı nesneleri üzerinde bir açıkalama/yorum yapmak istiyorsanız Comment'i kullanabiliriz.

    #endregion

    #region ConcurrencyCheck - IsConcurrencyToken

//Bir verinin aynı anda birden fazla işlem tarafından değiştirilmesini engellemek için kullanılan bir yapılandırmadır.
// Bir satırdaki verinin bütünsel olarak tutarlılığını sağlayacak bir yapılandırmadır.

    #endregion

    #region InverseProperty

//İki entity arasında birden fazla ilişki varsa eğer bu ilişkilerin hangi navigation property üzerinden olacağını ayarlamamızı sağlayan bir konfigrürasyondur.

    #endregion

    #endregion

    #region Composite Key

//Tablolarda birden fazla kolonu kümülatif olarak primary key yapmak istiyorsak buna composite key denir.

    #endregion

    #region HasDefaultSchema

//EF Core üzerinden inşa edilen herhangi bir veritabanı nesnesi default olarak dbo şemasına sahiptir. Bunu özelleştirebilmek için kullanılan bir yapılandırmadır.

    #endregion

    #region Property

    #region HasDefaultValue

//Tablodaki herhangi bir kolonun değer gönderilmediği durumlarda default olarak hangi değeri alacağını belirler.

    #endregion

    #region HasDefaultValueSql

//Tablodaki herhangi bir kolonun değer gönderilmediği durumlarda default olarak hangi sql cümleciğinden değeri alacağını belirler.

    #endregion

    #endregion

    #region HasComputedColumnSql

//Tablolarda birden fazla kolondaki verileri işleyerek değerini oluşturan kolonlara Computed Column denmektedir. EF Core üzerinden bu tarz computed column oluşturabilmek için kullanılan bir yapılandırmadır.

    #endregion

    #endregion büyükbirricın

    #region MyRegion OnModelCreating Metodu Nasıl Çalışır? Ne İşlev Görmektedir?

    // OnModelCreating metodu, veritabanı ilk defa oluşturulurken tetiklenen bir virtual metotdur.
    // DbContext içerisinde bulunur.
    // Code First yapısında DbContext’ten miras alarak oluşturduğumuz Context sınıfımızda override ederek kullanacağız.
    // Bu metod sayesinde veritabanı tabloları oluşturulmadan araya girecek, tablo isimlerine müdahale edebilecek veya kolonlara istediğimiz ayarları gerçekleştirebileceğiz.

    # endregion
}