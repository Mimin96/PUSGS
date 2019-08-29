 import { Component, OnInit } from '@angular/core';
 import { NgForm } from '@angular/forms';
 import { Router } from '@angular/router';
import { User } from 'src/app/temp-user/osoba';
import { AuthHttpService } from 'src/app/services/http/auth.service';
import { LoginService } from '../services/login.service';


@Component({
  selector: 'app-log-in',
  templateUrl: './log-in.component.html',
  styleUrls: ['./log-in.component.css']
})

export class LogInComponent implements OnInit {
  private submitted: boolean = false;
  constructor(private authService: AuthHttpService, private loginService: LoginService, private router: Router) { }

  ngOnInit() {
    if(this.authService.isLoggedIn()) {
      this.router.navigate(['/']);  // vrati korisnika na pocetnu stranu 
    }
  }

  logIn(user:User,form: NgForm){
    this.authService.logIn(user.username, user.password);
    //this.authService.logIn2(user.username,user.password);
    //form.reset();
    this.router.navigate(['/home']); 
  }

  
}