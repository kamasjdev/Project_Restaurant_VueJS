using Restaurant.Application.Exceptions;
using Restaurant.Domain.Entities;
using System.Text;

namespace Restaurant.Application.Mail
{
    public sealed class EmailMessage
    {
        private EmailMessage(string subject, string body) 
        {
            Subject = subject;
            Body = body;
        }

        public string Subject { get; }
        public string Body { get; }

        public sealed class EmailMessageBuilder
        {
            private StringBuilder _subject = new();
            private StringBuilder _body = new();
            private const string startTable = "<table style=\"border-collapse:collapse; text-align:center;\" >";
            private const string endTable = "</table>";
            private const string startHeaderRow = "<tr style=\"background-color:#6FA1D2; color:#ffffff;\">";
            private const string endHeaderRow = "</tr>";
            private const string startTr = "<tr style=\"color:#555555;\">";
            private const string endTr = "</tr>";
            private const string startTd = "<td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding: 5px;\">";
            private const string endTd = "</td>";

            public EmailMessage ConstructEmailFromOrder(Order order)
            {
                if (order == null)
                {
                    throw new CannotConstructEmailFromOrderException();
                }

                _subject.Append($"Zamówienie nr {order.OrderNumber}");
                _body.Append("<font>Nr zamówienia: ");
                _body.Append(order.OrderNumber);
                _body.Append(", data zamówienia: ");
                _body.Append(order.Created);
                _body.Append("</font><br><br>");
                _body.Append(startTable);
                _body.Append(startHeaderRow);
                _body.Append(startTd + "Nazwa Dania" + endTd);
                _body.Append(startTd + "Koszt" + endTd);
                _body.Append(endHeaderRow);

                foreach (var productSale in order.Products)
                {
                    AddProduct(productSale);
                }

                _body.Append(endTable);
                _body.Append("<br/>");

                if (order.Note is not null)
                {
                    AddNotes(order.Note);
                }

                _body.Append("<br/>");
                _body.AppendLine("\n<font>Koszt : " + order.Price + " zł" + "</font>");
                
                return new EmailMessage(_subject.ToString(), _body.ToString());
            }

            private void AddProduct(ProductSale productSale)
            {
                var product = new StringBuilder(startTr);
                product.Append(startTd);
                product.Append(productSale.Product.ProductName);
                product.Append(endTd);
                product.Append(startTd);
                product.Append(productSale.Product.Price);
                product.Append(" zł");
                product.Append(endTd);
                product.Append(endTr);

                if (productSale.Addition != null)
                {
                    product.Append(startTr);
                    product.Append(startTd);
                    product.Append(productSale.Addition.AdditionName);
                    product.Append(" zł");
                    product.Append(endTd);
                    product.Append(startTd);
                    product.Append(productSale.Addition.Price);
                    product.Append(" zł");
                    product.Append(endTd);
                    product.Append(endTr);
                }

                _body.Append(product);
            }

            private void AddNotes(string notes)
            {
                var note = new StringBuilder("<h5>");
                note.Append("Notes:");
                note.Append("</h5>");
                note.Append("<br/>");
                note.Append("<h5>");
                note.Append(notes);
                note.Append("</h5>");
                _body.Append(note);
            }
        }
    }
}
