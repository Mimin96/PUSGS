import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { LogInComponent } from './log-in/log-in.component';
import { PricelistComponent } from './pricelist/pricelist.component';
import { DrivingLinesComponent } from './driving-lines/driving-lines.component';
import { TimetableStationComponent } from './timetable-station/timetable-station.component';

const routes: Routes = [];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
