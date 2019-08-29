import { User } from 'src/app/temp-user/osoba';
import { Injectable } from '@angular/core';
//import { Http, Headers, RequestOptions } from '@angular/http'
import { HttpClient } from '@angular/common/http';

import { API_ADDRESS } from 'src/environments/appconfig';
import { Observable } from 'rxjs';

@Injectable()
export class RegisterService {
    private apiAddress: string = `${API_ADDRESS}/Account/Register`;

    constructor(private http: HttpClient) { }

    registerUser(user: User /*documentImage: File*/): Observable<any> {
        const formData: FormData = new FormData();
        formData.append('user', JSON.stringify(user));
        // if(documentImage !== null)
        //     formData.append('uploadFile', documentImage, documentImage.name);

        // const headers = new Headers();
        // headers.append('enctype', 'multipart/form-data');       
        // headers.append('Accept', 'application/json');

        //const options = new RequestOptions({ headers: headers });

        return this.http.post(this.apiAddress, formData/*, options*/);
    }

    

}