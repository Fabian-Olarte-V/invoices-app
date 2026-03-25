import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormBuilder } from '@angular/forms';
import { InvoiceFormComponent } from './invoice-form.component';
import { Client } from '../../interfaces/client.interface';
import { Product } from '../../interfaces/product.interface';

describe('InvoiceFormComponent', () => {
  let component: InvoiceFormComponent;
  let fixture: ComponentFixture<InvoiceFormComponent>;

  const clients: Client[] = [
    { id: 1, bussinessName: 'ACME Corp' }
  ];

  const products: Product[] = [
    { id: 7, name: 'Monitor', unitPrice: 100, ext: '.png', imageBase64: 'abc123' }
  ];

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InvoiceFormComponent],
      providers: [FormBuilder]
    }).compileComponents();

    fixture = TestBed.createComponent(InvoiceFormComponent);
    component = fixture.componentInstance;
    component.clients = clients;
    component.products = products;
    fixture.detectChanges();
  });

  it('requires at least one invoice detail', () => {
    component.invoiceForm.patchValue({
      clientId: '1',
      invoiceNumber: 'INV-001'
    });

    expect(component.invoiceForm.valid).toBeFalse();
    expect(component.invoiceForm.errors).toEqual({ noDetails: true });
  });

  it('updates unit price, image, and totals when a product is selected', () => {
    component.addDetail();
    component.details.at(0).patchValue({
      productId: '7',
      quantity: 2
    });

    component.onProductSelect(0);

    const detail = component.details.at(0);
    expect(detail.get('unitPrice')?.value).toBe(100);
    expect(detail.get('imageUrl')?.value).toBe('data:image/png;base64,abc123');
    expect(detail.get('total')?.value).toBe(200);
    expect(component.subtotal()).toBe(200);
    expect(component.tax()).toBe(38);
    expect(component.total()).toBe(238);
  });

  it('emits a normalized invoice request on submit', () => {
    spyOn(component.submitInvoice, 'emit');

    component.addDetail();
    component.invoiceForm.patchValue({
      clientId: '1',
      invoiceNumber: 'INV-001'
    });
    component.details.at(0).patchValue({
      productId: '7',
      quantity: 3,
      unitPrice: 100
    });
    component.onSubmit();

    expect(component.submitInvoice.emit).toHaveBeenCalledWith({
      invoiceData: {
        clientId: 1,
        invoiceNumber: 'INV-001',
        items: [
          {
            productId: 7,
            quantity: 3,
            unitPrice: 100
          }
        ]
      }
    });
  });

  it('resets the form and removes all details', () => {
    component.addDetail();
    component.invoiceForm.patchValue({
      clientId: '1',
      invoiceNumber: 'INV-001'
    });
    component.details.at(0).patchValue({
      productId: '7',
      quantity: 1,
      unitPrice: 100
    });
    component.onProductSelect(0);

    component.resetForm();

    expect(component.details.length).toBe(0);
    expect(component.invoiceForm.get('clientId')?.value).toBe('');
    expect(component.invoiceForm.get('invoiceNumber')?.value).toBe('');
    expect(component.subtotal()).toBe(0);
    expect(component.total()).toBe(0);
  });
});
