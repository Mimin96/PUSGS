import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { of } from 'rxjs';


@Injectable()
export class HttpService
{
    getName() : Observable<string>{
        return of("Neko ime iz servisa");
    }
}