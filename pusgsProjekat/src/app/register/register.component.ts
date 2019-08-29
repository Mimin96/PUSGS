import { Component, OnInit } from '@angular/core';
import { Validators } from '@angular/forms';
import { FormArray } from '@angular/forms';
import { AuthHttpService } from 'src/app/services/http/auth.service';
import { User, Osoba } from 'src/app/temp-user/osoba';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { FormBuilder } from '@angular/forms';
import { RegisterService } from '../services/register.service';
declare var getDate: any;
declare var initDatePicker: any;
@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  private submitted: boolean = false;
  private user: User;
  private role: string;  
  private fileToUpload: File = null;
  //private userTypes: UserType[];
  private showImageInput: boolean = false;
  private types = ['Regular', 'Controlor', 'Admin'];
  registacijaForm = this.fb.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    username: ['', Validators.required],
    password: ['', Validators.required],
    confirmPassword: ['', Validators.required],
    email: ['', Validators.required],
    date: ['', Validators.required],
    tip: ['', Validators.required]
  });
  
  constructor(private http: AuthHttpService, private fb: FormBuilder, private router: Router, private registerService: RegisterService) { }
  
    ngOnInit() {
      
    }
  
    register(){
      let regModel: User = this.registacijaForm.value;
      this.http.registration(regModel);
      this.router.navigate(['/login']); //login
      //form.reset();
    }

    onSubmit(f: NgForm) {
      // Check for date value
      let date = getDate();
      
      // jesus christ man
      this.user = new User(f.value.firstName, f.value.lastName, f.value.email, f.value.username, 
                           f.value.password, f.value.confirmPassword, f.value.address,
                           date);





      console.log(this.user);
      
      this.submitted = true;  // animation
  
      // admin adding a new ticket inspector? 
      
  
      
      this.registerService.registerUser(this.user/*, this.fileToUpload*/).subscribe( 
        (response) => {
          this.submitted = false;  // animation
  
            //this.notificationService.notifyEvent.emit('Successfully registered. You can now log in.');
            
        },
    
          /*(error) => {       
            this.submitted = false;  // animation
            //this.notificationService.notifyEvent.emit('An error ocurred during registration. Please, try again.');
  
            if(error.status !== 0){
              // Notify the user about errors from WebAPI (validation error reply)
              let regReply = JSON.parse(error._body);
              let errorMessages = Object.values(regReply.ModelState);
              if(errorMessages.length > 0) {
                for(let i = 0; i < errorMessages.length; i++){
                  this.notificationService.notifyEvent.emit(errorMessages[i][0]);
                }
              }
            } 
  
          }*/
        );
        this.router.navigate(['/login']); //login
      }
      
    
      
    }

