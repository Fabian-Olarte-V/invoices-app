import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Client } from '../../interfaces/client.interface';
import { Product } from '../../interfaces/product.interface';
import { InvoiceFormComponent } from '../../components/invoice-form/invoice-form.component';
import { InvoiceCreateRequest } from '../../interfaces/invoice.interface';
import { ClientsService } from '../../services/clients/clients-service';
import { ProductsService } from '../../services/products/products-service';
import { InvoicesService } from '../../services/invoices/invoices-service';
import { AlertService } from '../../../../shared/services/alert.service';


@Component({
  selector: 'app-create-invoice-view',
  standalone: true,
  imports: [CommonModule, InvoiceFormComponent],
  templateUrl: './create-invoice-view.component.html',
  styleUrls: ['./create-invoice-view.component.scss']
})
export class CreateInvoiceView implements OnInit {
  @ViewChild('invoiceFormComponent') InvoiceFormComponent!: InvoiceFormComponent;
  clients: Client[] = [];
  products: Product[] = [];

  constructor(
    private clientsService: ClientsService, 
    private productsService: ProductsService, 
    private invoicesService: InvoicesService,
    private alertService: AlertService
  ) {}


  ngOnInit() {
    this.loadClients();
    this.loadProducts();
  }

  private loadClients(): void {
    this.clientsService.getClients().subscribe({
      next: (clients) => this.clients = clients
    });
  }

  private loadProducts(): void {
    this.productsService.getProducts().subscribe({
      next: (products) => this.products = products
    });
  }

  onInvoiceSubmit(invoiceData: InvoiceCreateRequest) {
    this.invoicesService.createInvoice(invoiceData).subscribe({
      next: (response) => {
        this.alertService.success(`Factura ${response.invoiceNumber} creada exitosamente!`);
        this.resetForm();
      },
      error: (error) => {
        if (error.status === 409) {
          this.alertService.error(`Ya existe una factura con el número ${invoiceData.invoiceData.invoiceNumber}.`);
        }
      },
      complete: () => {
        this.resetForm();
      }
    });
  }

  resetForm() {
    if (this.InvoiceFormComponent) {
      this.InvoiceFormComponent.resetForm();
    }
  }
}
