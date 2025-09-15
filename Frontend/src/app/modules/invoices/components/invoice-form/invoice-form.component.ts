import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AbstractControl, FormArray, FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { computed, signal } from '@angular/core';
import { Client } from '../../interfaces/client.interface';
import { Product } from '../../interfaces/product.interface';
import { InvoiceCreateRequest, InvoiceDetail } from '../../interfaces/invoice.interface';

@Component({
  selector: 'app-invoice-form',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './invoice-form.component.html',
  styleUrls: ['./invoice-form.component.scss']
})
export class InvoiceFormComponent {
  @Input() clients: Client[] = [];
  @Input() products: Product[] = [];
  @Output() submitInvoice = new EventEmitter<InvoiceCreateRequest>();

  private detailsSignal = signal<any[]>([]);
  invoiceForm: FormGroup;

  subtotal = computed(() => this.detailsSignal().reduce((sum: number, detail: any) => sum + (detail.total || 0), 0));
  tax = computed(() => this.subtotal() * 0.19);
  total = computed(() => this.subtotal() + this.tax());

  constructor(private fb: FormBuilder) {
    this.invoiceForm = this.fb.nonNullable.group({
      clientId: this.fb.nonNullable.control('', { validators: [Validators.required] }),
      invoiceNumber: this.fb.nonNullable.control('', { validators: [Validators.required, Validators.min(1)] }),
      details: this.fb.nonNullable.array([])
    }, {
      validators: this.validateDetails
    });
  }

  get details(): FormArray {
    return this.invoiceForm.get('details') as FormArray;
  }

  onSubmit(): void {
    if (this.invoiceForm?.valid) {
      const items: InvoiceDetail[] = this.details.controls.map(control => ({
        productId: +control.get('productId')?.value,
        quantity: +control.get('quantity')?.value,
        unitPrice: +control.get('unitPrice')?.value
      }));

      const request: InvoiceCreateRequest = {
        invoiceData: {
          clientId: +this.invoiceForm.get('clientId')?.value,
          invoiceNumber: this.invoiceForm.get('invoiceNumber')?.value,
          items
        }
      };

      this.submitInvoice.emit(request);
    }
  }

  onProductSelect(index: number): void {
    const detailForm = this.details.at(index);
    const productId = detailForm.get('productId')?.value;
    const product = this.products.find(p => p.id === +productId);
    
    if (product) {
      const imageUrl = `data:image/${product.ext?.replace(".","").trim()};base64,${product.imageBase64}`;
      detailForm.patchValue({
        unitPrice: product.unitPrice,
        imageUrl: imageUrl
      });

      this.calculateDetailTotal(index);
    }
  }

  addDetail(): void {
    const detailForm = this.fb.group({
      productId: ['', Validators.required],
      quantity: [1, [Validators.required, Validators.min(1)]],
      unitPrice: [0],
      imageUrl: [''],
      total: [0]
    });

    detailForm.get('quantity')?.valueChanges.subscribe(() => {
      this.calculateDetailTotal(this.details.length - 1);
    });

    this.details.push(detailForm);
    this.invoiceForm.setControl('details', this.details);
    this.invoiceForm.updateValueAndValidity();
  }

  removeDetail(index: number): void {
    this.details.removeAt(index);
    const currentDetails = [...this.detailsSignal()];
    currentDetails.splice(index, 1);
    this.detailsSignal.set(currentDetails);
    this.invoiceForm.updateValueAndValidity();
  }

  private validateDetails = (control: AbstractControl): ValidationErrors | null => {
    const details = control.get('details') as FormArray;
    return details?.length === 0 ? { 'noDetails': true } : null;
  }

  private calculateDetailTotal(index: number): void {
    const detail = this.details.at(index);
    const quantity = detail.get('quantity')?.value || 0;
    const unitPrice = detail.get('unitPrice')?.value || 0;
    const total = quantity * unitPrice;
    
    detail.get('total')?.setValue(total, { emitEvent: false });
    
    const currentDetails = this.detailsSignal();
    if (currentDetails.length <= index) {
      currentDetails.push({});
    }
    
    currentDetails[index] = {
      productId: detail.get('productId')?.value,
      quantity,
      unitPrice,
      total,
      imageUrl: detail.get('imageUrl')?.value
    };
    
    this.detailsSignal.set([...currentDetails]);
  }

  resetForm(): void {
    this.invoiceForm?.reset();
    
    while (this.details?.length !== 0) {
      this.details.removeAt(0);
    }
    
    this.invoiceForm?.patchValue({
      clientId: '',
      invoiceNumber: ''
    });
    
    this.detailsSignal.set([]);
  }
}


