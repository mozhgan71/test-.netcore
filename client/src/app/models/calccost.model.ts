export interface CalcCost {
    id?: string,
    driverName?: string,
    dateRefueling?: string,
    amount: number,
    totalAmount?: number,
    rate: number,
    typeCar: string,
    payable: number,
    totalPay?: number,
}