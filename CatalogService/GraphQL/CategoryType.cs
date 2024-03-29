﻿using DataAccessLayer.Entities;

namespace CatalogService.GraphQL
{
    public class CategoryType : ObjectType<Category>
    {
        protected override void Configure(IObjectTypeDescriptor<Category> descriptor)
        {
            descriptor.Field(c => c.Id).Type<IdType>();
            descriptor.Field(c => c.Name).Type<StringType>();
            descriptor.Field(c => c.Image).Type<StringType>();
            descriptor.Field(c => c.ParentCategoryId).Type<IntType>();
        }
    }
}
