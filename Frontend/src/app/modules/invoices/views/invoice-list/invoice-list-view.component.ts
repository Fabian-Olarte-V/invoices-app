import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InvoiceListComponent } from '../../components/invoice-list/invoice-list.component';
import { ClientsService } from '../../services/clients/clients-service';
import { InvoicesService } from '../../services/invoices/invoices-service';
import { Client } from '../../interfaces/client.interface';
import { Invoice, InvoiceSearchCriteria } from '../../interfaces/invoice.interface';
import { finalize } from 'rxjs';
import { AlertService } from '../../../../shared/services/alert.service';

@Component({
  selector: 'app-invoice-list-view',
  imports: [CommonModule, InvoiceListComponent],
  templateUrl: './invoice-list-view.component.html',
  styleUrls: ['./invoice-list-view.component.scss']
})
export class InvoiceListView implements OnInit {
  @ViewChild('invoiceListComponent') InvoiceListComponent!: InvoiceListComponent;
  clients: Client[] = [];
  invoices: Invoice[] = [];

  constructor(
    private clientsService: ClientsService,
    private invoicesService: InvoicesService,
  ) {}


  ngOnInit(): void {
    this.loadClients();
  }

  onSearch(criteria: InvoiceSearchCriteria): void {
    if (criteria.searchType === 'client' && criteria.clientId) {
      this.searchByClient(criteria.clientId);
    } else if (criteria.searchType === 'invoice' && criteria.invoiceNumber) {
      this.searchByInvoiceNumber(criteria.invoiceNumber);
    }
  }

  private loadClients(): void {
    this.clientsService.getClients().subscribe({
      next: (clients) => {
        this.clients = clients;
      }
    });
  }

  private searchByClient(clientId: number): void {
    this.invoicesService.getInvoicesByClientId(clientId)
      .pipe(
        finalize(() => this.InvoiceListComponent.changeLoadingStatus(false))
      )
      .subscribe({
        next: (invoices) => this.invoices = invoices,
        error: () => this.invoices = [],
      });
  }

  private searchByInvoiceNumber(invoiceNumber: string): void {
    this.invoicesService.getInvoiceByNumber(invoiceNumber)
      .pipe(
        finalize(() => this.InvoiceListComponent.changeLoadingStatus(false))
      )
      .subscribe({
        next: (invoice) => this.invoices = [invoice],
        error: () => this.invoices = [],
      });
  }
}

