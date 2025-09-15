import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Client } from '../../interfaces/client.interface';
import { API_BASE_URL } from '../../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ClientsService {
  private readonly apiUrl = `${API_BASE_URL}/clients`;

  constructor(private http: HttpClient) {}

  getClients(): Observable<Client[]> {
    return this.http.get<Client[]>(this.apiUrl);
  }
}
