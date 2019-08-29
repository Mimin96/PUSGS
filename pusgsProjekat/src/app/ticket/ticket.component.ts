import { Component, OnInit } from '@angular/core';
import { AuthHttpService } from '../services/http/auth.service';

@Component({
  selector: 'app-ticket',
  templateUrl: './ticket.component.html',
  styleUrls: ['./ticket.component.css']
})
export class TicketComponent implements OnInit {

  constructor(private http: AuthHttpService) { }
  tipovi: string[] = ["Dnevna", "Mesecna", "Godisnja", "Vremenska"];
  tip: string;
  cena1: number;
  vaziDo1 : string;
  user: string;
  korisnik: string;
  
  ngOnInit() {

    let jwtData = localStorage.jwt.split('.')[1]
        //let decodedJwtJsonData = window.atob(jwtData)
        //let decodedJwtData = JSON.parse(decodedJwtJsonData)
        //this.user = decodedJwtData.nameid;
        if(jwtData == undefined)
        {
          this.user = "neregistrovan";
        }
        else
        {
          let decodedJwtJsonData = window.atob(jwtData)
        let decodedJwtData = JSON.parse(decodedJwtJsonData)
        this.user = decodedJwtData.nameid;
        }
        this.http.GetTipKorisnika(this.user).subscribe((korisnik)=>{
          this.korisnik = korisnik;
          err => console.log(err);
        });
  }

  CenaKarte(){
    this.http.GetCenaKarte(this.tip).subscribe((cena)=>{
      this.cena1 = cena;
      err => console.log(err);
    });
  }
  KupiKartu(){
     let jwtData = localStorage.jwt.split('.')[1]
        let decodedJwtJsonData = window.atob(jwtData)
        let decodedJwtData = JSON.parse(decodedJwtJsonData)


       
        this.user = decodedJwtData.nameid;
        
      this.http.GetKupiKartu(this.tip, "student", this.user).subscribe((vaziDo)=>
    {
      this.vaziDo1 = vaziDo;
      err => console.log(err);
      });

  }

}
