using DataAccessLayer.Entities;

namespace CatalogService.GraphQL
{
    public class ItemType : ObjectType<Item>
    {
        protected override void Configure(IObjectTypeDescriptor<Item> descriptor)
        {
            descriptor.Field(c => c.Id).Type<IdType>();
            descriptor.Field(c => c.Name).Type<StringType>();
            descriptor.Field(c => c.Description).Type<StringType>();
            descriptor.Field(c => c.Image).Type<StringType>();
            descriptor.Field(c => c.CategoryId).Type<IntType>();
            descriptor.Field(c => c.Price).Type<DecimalType>();
            descriptor.Field(c => c.Amount).Type<IntType>();
        }
    }
    public class ItemQueryType : ObjectType<Query>
    {
        protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
        {
            descriptor.Field(q => q.GetItems()).Type<ListType<ItemType>>();
        }
    }
}
