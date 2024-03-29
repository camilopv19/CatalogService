﻿using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.CoreLogic
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _repository;
        public ItemService(IItemRepository repository)
        {
            _repository = repository;
        }
        public IEnumerable<ItemResponse> List(int? categoryId, int? page) => _repository.List(categoryId, page);
        public IEnumerable<ItemWithCategory> GetItemsWithCategory() => _repository.GetItemsWithCategory();
        public IEnumerable<Item> List() => _repository.List();

        public Item? Get(int id) => _repository.Get(id);
        public int Delete(int id) => _repository.Delete(id);
        public int Upsert(Item item) => _repository.Upsert(item);

    }
}
