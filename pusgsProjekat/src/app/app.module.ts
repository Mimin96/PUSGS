import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LogInComponent } from './log-in/log-in.component';
import { HomeComponent } from './home/home.component';
import { ProfilEditorComponent } from './profil-editor/profil-editor.component';
import { ValidatorEditorComponent } from './validator-editor/validator-editor.component';
import { NavbarComponent } from './navbar/navbar.component';
import { RegisterComponent } from './register/register.component';
import { PricelistComponent } from './pricelist/pricelist.component';
import { DrivingLinesComponent } from './driving-lines/driving-lines.component';

import { HttpClientModule }    from '@angular/common/http';
import { TimetableStationComponent } from './timetable-station/timetable-station.component';

//import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { RouterModule, Routes} from '@angular/router'
import { TempUserComponent } from './temp-user/temp-user.component';
import { HttpService } from 'src/app/services/http.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { TokenInterceptor } from 'src/app/interceptors/token.interceptor';
import { FormsModule} from '@angular/forms'
import { AuthHttpService } from 'src/app/services/http/auth.service';
import { ReactiveFormsModule } from '@angular/forms';
import { RedVoznjeHttpService } from './services/redvoznje.service';
import { TicketComponent } from './ticket/ticket.component';
import { RegisterService } from './services/register.service';
import { LoginService } from './services/login.service';
import { ProfilComponent } from './profil/profil.component';
import { AuthComponent } from './auth/auth.component';

const routes : Routes = [
  {path:"home", component: HomeComponent},
  {path:"login", component: LogInComponent},
  {path:"registration", component: RegisterComponent},
  {path:"pricelist", component: PricelistComponent},
  {path:"timetable-station", component: TimetableStationComponent},
  {path:"driving-lines", component: DrivingLinesComponent},
  {path:"tickets", component: TicketComponent},
  {path:"profil", component: ProfilComponent},
  {path: "", component: HomeComponent, pathMatch: "full"},
  {path: "**", redirectTo: "home"}
]

@NgModule({
  declarations: [
    AppComponent,
    LogInComponent,
    HomeComponent,
    ProfilEditorComponent,
    ValidatorEditorComponent,
    NavbarComponent,
    RegisterComponent,
    PricelistComponent,
    DrivingLinesComponent,
    TimetableStationComponent,
    TempUserComponent,
    TicketComponent,
    ProfilComponent,
    AuthComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot(routes)
  ],
  providers: [HttpService, {provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true},AuthHttpService,RedVoznjeHttpService, RegisterService, LoginService ],
  bootstrap: [AppComponent]
})
export class AppModule { }
