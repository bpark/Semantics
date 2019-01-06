import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {DataModel} from "./data-model";

@Injectable({
  providedIn: 'root'
})
export class RdfDataService {

  constructor(private http: HttpClient) { }

  get(): Observable<DataModel> {
    return this.http.get<DataModel>(RdfDataService.createConnectionUrl());
  }

  private static createConnectionUrl(): string {
    return 'https://localhost:5001/api/financial';
  }
}
