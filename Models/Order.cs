﻿using System.ComponentModel.DataAnnotations;
using Stateless;

namespace FakeXiechengAPI.Models
{
    // 枚举——订单的状态
    public enum OrderStateEnum
    {
        Pending,    // 订单已生成
        Processing, // 支付处理中
        Completed,  // 交易成功
        Declined,   // 交易失败
        Cancelled,  // 订单取消
        Refund, // 已退款
    }

    // 枚举——订单状态的触发动作
    public enum OrderStateTriggerEnum
    {
        PlaceOrder, // 支付
        Approve,    // 支付成功
        Reject,     // 支付失败
        Cancel,     // 取消
        Return      // 退货
    }
    public class Order
    {
        public Order()
        {
            StateMachineInit();
        }
        [Key]
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<LineItem> OrderItems { get; set; }
        public OrderStateEnum State { get; set; }
        public DateTime CreateDateUTC { get; set; }
        public string TransactionMetadata { get; set; } // 支付金额，一般是以API回调的方式传递给支付系统
        StateMachine<OrderStateEnum, OrderStateTriggerEnum> _machine;

        public void PaymentProcessing()
        {
            _machine.Fire(OrderStateTriggerEnum.PlaceOrder);
        }

        public void PaymentApprove()
        {
            _machine.Fire(OrderStateTriggerEnum.Approve);
        }

        public void PaymentReject()
        {
            _machine.Fire(OrderStateTriggerEnum.Reject);
        }

        private void StateMachineInit()
        {
            // 初始化状态机
            _machine = new StateMachine<OrderStateEnum, OrderStateTriggerEnum>(() => State, s => State = s);

            _machine.Configure(OrderStateEnum.Pending)
                .Permit(OrderStateTriggerEnum.PlaceOrder, OrderStateEnum.Processing)
                .Permit(OrderStateTriggerEnum.Cancel, OrderStateEnum.Cancelled);

            _machine.Configure(OrderStateEnum.Processing)
                .Permit(OrderStateTriggerEnum.Approve, OrderStateEnum.Completed)
                .Permit(OrderStateTriggerEnum.Reject, OrderStateEnum.Declined);

            _machine.Configure(OrderStateEnum.Declined)
                .Permit(OrderStateTriggerEnum.PlaceOrder, OrderStateEnum.Processing);

            _machine.Configure(OrderStateEnum.Completed)
                .Permit(OrderStateTriggerEnum.Return, OrderStateEnum.Refund);
        }
    }
}
