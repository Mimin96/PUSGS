export class Osoba{
    name: string
    lastname: string
}

export class User{
    username:string
    password:string
    firstName:string
    lastName:string
    email:string
    confirmpassword:string
    date:string
    tip:string
    //address:string

    constructor(username?:string, firstName?: string, lastName?: string, email?: string, 
        password?: string, confirmPassword?: string, date?: string, tip?: string) {

    this.firstName = firstName;
    this.lastName = lastName;
    this.email = email;    
    this.password = password;
    this.confirmpassword = confirmPassword;
    //this.address = address;
    this.username = username;
    this.date = date;
    this.tip = tip;
}
}