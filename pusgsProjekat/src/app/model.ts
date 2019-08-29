export class Day{
    Id : number
    DayOfWeek : string
}
export class Line{
    Id : number
    Number : number
}

export class Pricelist{
    Id : number
    From : string
    To : string
}

export class PriceOfTicket{
    Id : number
    Price : number
}

export class Station{
    Id : number
    Name : string
    Address: string
    X : number
    Y : number
}

export class Timetable{
    Id : number
    Info : string
}

export class TimetableInfo{
    Day: Day[];
    Lines: Line[];
}

export class TimetableType{
    Id: number
    Name: string
}