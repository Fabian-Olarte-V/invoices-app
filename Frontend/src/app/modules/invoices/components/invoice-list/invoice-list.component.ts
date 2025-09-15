import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { Client } from '../../interfaces/client.interface';
import { Invoice, InvoiceSearchCriteria } from '../../interfaces/invoice.interface';

@Component({
  selector: 'app-invoice-list',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './invoice-list.component.html',
  styleUrls: ['./invoice-list.component.scss']
})
export class InvoiceListComponent {
  @Output() search = new EventEmitter<InvoiceSearchCriteria>();
  @Input() clients: Client[] = [];
  @Input() invoices: Invoice[] = [];

  searchForm!: FormGroup;
  searchType: 'client' | 'invoice' = 'client';
  isLoading: boolean = false;
  hasSearched: boolean = false;

  constructor(private fb: FormBuilder) {
    this.initializeForm();
  }

  private initializeForm(): void {
    this.searchForm = this.fb.nonNullable.group({
      searchType: this.fb.nonNullable.control('client'),
      clientId: this.fb.nonNullable.control(''),
      invoiceNumber: this.fb.nonNullable.control(''),
    });

    const clientControl = this.searchForm.get('clientId');
    const invoiceControl = this.searchForm.get('invoiceNumber');
    clientControl?.setValidators([Validators.required]);
    invoiceControl?.clearValidators();
    
    this.searchForm.updateValueAndValidity();

    this.searchForm.get('searchType')?.valueChanges.subscribe((value) => {
      this.searchType = value;
      if (value === 'client') {
        invoiceControl?.clearValidators();
        clientControl?.setValidators([Validators.required]);
        this.searchForm.patchValue({ invoiceNumber: '' });
      } else {
        clientControl?.clearValidators();
        invoiceControl?.setValidators([Validators.required, Validators.min(1)]);
        this.searchForm.patchValue({ clientId: '' });
      }
      
      clientControl?.updateValueAndValidity();
      invoiceControl?.updateValueAndValidity();
      this.searchForm.updateValueAndValidity();
    });
  }

  onSearch() {
    if (this.searchForm.valid) {
      const searchType = this.searchForm.get('searchType')?.value;
      const criteria: InvoiceSearchCriteria = {
        searchType,
        clientId: searchType === 'client' ? this.searchForm.get('clientId')?.value : null,
        invoiceNumber: searchType === 'invoice' ? this.searchForm.get('invoiceNumber')?.value : null
      };

      this.changeLoadingStatus(true);
      this.search.emit(criteria);
      this.hasSearched = true;
    }
  }

  changeLoadingStatus(isLoading: boolean) {
    this.isLoading = isLoading;
  }
}
