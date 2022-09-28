﻿using Restaurant.Domain.Exceptions;
using Restaurant.Domain.ValueObjects;

namespace Restaurant.Domain.Entities
{
    public class Addition
    {
        public EntityId Id { get; }
        public AdditionName AdditionName { get; private set; }
        public Price Price { get; private set; }
        public AdditionKind AdditionKind { get; private set; }

        private IList<EntityId> _productSaleIds = new List<EntityId>();
        public IEnumerable<EntityId> ProductSaleIds => _productSaleIds;

        public Addition(EntityId id, AdditionName additionName, Price price, AdditionKind additionKind, IEnumerable<EntityId> productSaleIds = null)
        {
            Id = id;
            ChangeAdditionName(additionName);
            ChangePrice(price);
            AdditionKind = additionKind;
            _productSaleIds = productSaleIds?.ToList() ?? new List<EntityId>();
        }

        public Addition(EntityId id, AdditionName additionName, Price price, string additionKind, IEnumerable<EntityId> productSaleIds = null)
        {
            Id = id;
            ChangeAdditionName(additionName);
            ChangePrice(price);
            ChangeAdditionKind(additionKind);
            _productSaleIds = productSaleIds?.ToList() ?? new List<EntityId>();
        }

        public void ChangeAdditionName(AdditionName additionName)
        {
            AdditionName = additionName;
        }

        public void ChangePrice(Price price)
        {
            Price = price;
        }

        public void ChangeAdditionKind(string additionKind)
        {
            var parsed = Enum.TryParse<AdditionKind>(additionKind, out var additionKindParsed);

            if (!parsed)
            {
                throw new InvalidAdditionKindException(additionKind);
            }

            if (!Enum.IsDefined(additionKindParsed))
            {
                throw new InvalidAdditionKindException(additionKind);
            }

            AdditionKind = additionKindParsed;
        }
    }
}