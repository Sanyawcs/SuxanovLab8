using System;
using System.Collections.Generic;
using System.Linq;

public class OrderProcessor
{
    public void ProcessOrders(List<Order> orders)
    {
        if (orders == null || orders.Count == 0)
        {
            Console.WriteLine("No orders to process.");
            return;
        }

        ProcessOrderStatus(orders, "New", "Processed", "Processing");
        ProcessOrderStatus(orders, "Processed", "Shipped", "Shipping");
    }

    private void ProcessOrderStatus(List<Order> orders, string currentStatus, string newStatus, string action)
    {
        var filteredOrders = orders.Where(o => o.Status == currentStatus).ToList();

        foreach (var order in filteredOrders)
        {
            Console.WriteLine($"{action} Order ID: {order.Id}");
            order.Status = newStatus;
        }
    }
}

public class Order
{
    public int Id { get; set; }
    public string Status { get; set; }
}