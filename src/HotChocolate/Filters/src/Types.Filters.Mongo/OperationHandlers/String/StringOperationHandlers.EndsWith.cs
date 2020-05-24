using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using HotChocolate.Language;
using HotChocolate.Types.Filters.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HotChocolate.Types.Filters.Mongo
{
    public static partial class StringOperationHandlers
    {
        public static bool EndsWith(
            FilterOperation operation,
            IInputType type,
            IValueNode value,
            IFilterVisitorContext<FilterDefinition<BsonDocument>> context,
            [NotNullWhen(true)]out FilterDefinition<BsonDocument>? result)
        {
            object parsedValue = type.ParseLiteral(value);

            if (parsedValue == null)
            {
                context.ReportError(
                    ErrorHelper.CreateNonNullError(operation, type, value, context));

                result = null;
                return false;
            }

            if (operation.Type == typeof(string) &&
                type.IsInstanceOfType(value) &&
                parsedValue is string str &&
                context is MongoFilterVisitorContext ctx)
            {
                result = ctx.Builder.Regex(
                    ctx.GetMongoFilterScope().GetPath(),
                    new BsonRegularExpression($"/{Regex.Escape(str)}$/"));

                return true;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public static bool NotEndsWith(
            FilterOperation operation,
            IInputType type,
            IValueNode value,
            IFilterVisitorContext<FilterDefinition<BsonDocument>> context,
            [NotNullWhen(true)]out FilterDefinition<BsonDocument>? result)
        {
            object parsedValue = type.ParseLiteral(value);

            if (parsedValue == null)
            {
                context.ReportError(
                    ErrorHelper.CreateNonNullError(operation, type, value, context));

                result = null;
                return false;
            }

            if (operation.Type == typeof(string) &&
                type.IsInstanceOfType(value) &&
                parsedValue is string str &&
                context is MongoFilterVisitorContext ctx)
            {
                result = ctx.Builder.Not(
                    ctx.Builder.Regex(
                        ctx.GetMongoFilterScope().GetPath(),
                        new BsonRegularExpression($"/{Regex.Escape(str)}$/")));

                return true;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
