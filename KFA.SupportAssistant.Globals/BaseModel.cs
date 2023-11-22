using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.SharedKernel;

namespace KFA.SupportAssistant.Globals;

	//
	// Summary:
	//     A base class for DDD Entities. Includes support for domain events dispatched
	//     post-persistence. If you prefer GUID Ids, change it here. If you need to support
	//     both GUID and int IDs, change to EntityBase<TId> and use TId as the type for
	//     Id.
	public abstract class BaseModel: IAggregateRoot
	{
		private List<DomainEventBase> _domainEvents = new List<DomainEventBase>();

		public string Id { get; set; }

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
	}
