import { Injectable } from '@angular/core';
import { API_BASE_URL } from '../../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Invoice, InvoiceCreateRequest } from '../../interfaces/invoice.interface';

@Injectable({
  providedIn: 'root'
})
export class InvoicesService {
  private readonly apiUrl = `${API_BASE_URL}/invoices`;

  constructor(private http: HttpClient) {}

  getInvoicesByClientId(clientId: number): Observable<Invoice[]> {
    return this.http.get<Invoice[]>(`${this.apiUrl}/by-client/${clientId}`);
  }

  getInvoiceByNumber(invoiceNumber: string): Observable<Invoice> {
    return this.http.get<Invoice>(`${this.apiUrl}/${invoiceNumber}`);
  }

  createInvoice(invoiceData: InvoiceCreateRequest): Observable<Invoice> {
    return this.http.post<Invoice>(this.apiUrl, invoiceData);
  }
}
