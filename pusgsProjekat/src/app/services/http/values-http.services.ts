import { BaseHttpService } from 'src/app/services/http/base-http.service';



export class ValuesHttpService extends BaseHttpService<string>{
    specificUrl = "/api/values"

}