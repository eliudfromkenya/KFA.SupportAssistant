using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Ardalis.SharedKernel;
using Newtonsoft.Json;

namespace KFA.SupportAssistant.Globals;

	//
	// Summary:
	//     A base class for DDD Entities. Includes support for domain events dispatched
	//     post-persistence. If you prefer GUID Ids, change it here. If you need to support
	//     both GUID and int IDs, change to EntityBase<TId> and use TId as the type for
	//     Id.
	public abstract record class BaseModel: IAggregateRoot
	{
		private readonly List<DomainEventBase> _domainEvents = [];
  private string? _id;

  [Key]
  [MaxLength(___PrimaryMaxLength___)]
  [Column("id")]
  public virtual string? Id { get => _id; set => _id = value; }

  public BaseModel(string? id = null) => _id = id;
  
		[NotMapped]
		public IEnumerable<DomainEventBase> DomainEvents => _domainEvents.AsReadOnly();

		protected void RegisterDomainEvent(DomainEventBase domainEvent)
		{
			_domainEvents.Add(domainEvent);
		}

		internal void ClearDomainEvents()
		{
			_domainEvents.Clear();
		}

  public abstract object ToBaseDTO();

#pragma warning disable IDE1006 // Naming Styles
  internal const int ___PrimaryMaxLength___ = 20;
#pragma warning restore IDE1006 // Naming Styles

  [NotMapped]
  [JsonIgnore]
  public abstract string? ___tableName___ { get; protected set; }

  [NotMapped]
  [JsonIgnore]
  public bool? ___RecordIsSelected___ { get; set; }

  //[Column("modification_status", Order = 103)]
  //public byte? ___ModificationStatus___ { get; set; } = 1;

  [NotMapped]
  [JsonIgnore]
  public object? ___Tag___ { get; set; }

  [Column("date_added", Order = 100)]
  public long? ___DateInserted___ { get; set; } = DateTime.Now.FromDateTime();

  [Column("date_updated", Order = 101)]
  public long? ___DateUpdated___ { get; set; } = DateTime.Now.FromDateTime();
  }
