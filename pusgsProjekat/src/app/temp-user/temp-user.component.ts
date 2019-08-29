import { Component, OnInit } from '@angular/core';
import { HttpService } from 'src/app/services/http.service';
import { ValuesHttpService } from 'src/app/services/http/values-http.services';
import { AuthHttpService } from 'src/app/services/http/auth.service';

@Component({
  selector: 'app-temp-user',
  templateUrl: './temp-user.component.html',
  styleUrls: ['./temp-user.component.css'],
  providers: [ValuesHttpService, AuthHttpService]
})
export class TempUserComponent implements OnInit {

  name: string
  clicks: number
  values: string[]
  constructor(private http: ValuesHttpService, private auth: AuthHttpService) { }

  ngOnInit() {
    this.name = "salo";

    
    this.http.getAll().subscribe((values) => this.values = values, err => console.log(err));
    this.clicks = 0;
  }

  clickCounter(){
    this.clicks++;
  }

}
