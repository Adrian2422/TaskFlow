import { Component, signal } from '@angular/core';
import {
  ColumnDef,
  createAngularTable,
  FlexRenderDirective,
  getCoreRowModel,
} from '@tanstack/angular-table';
import { ZardTableComponent } from '@/shared/ui/components/table';
import { ZardCardComponent } from '@/shared/ui/components/card';
import { ZardButtonComponent } from '@/shared/ui/components/button';
import { ZardIconComponent } from '@/shared/ui/components/icon';

interface User {
  id: number;
  name: string;
  email: string;
  role: string;
}

@Component({
  selector: 'app-oldest-tasks',
  imports: [
    FlexRenderDirective,
    ZardTableComponent,
    ZardCardComponent,
    ZardButtonComponent,
    ZardIconComponent,
  ],
  templateUrl: './oldest-tasks.html',
  styleUrl: './oldest-tasks.scss',
  host: {
    class: 'flex flex-1',
  },
})
export class OldestTasks {
  readonly data = signal<User[]>([
    { id: 1, name: 'Jan Kowalski', email: 'jan@example.com', role: 'Admin' },
    { id: 2, name: 'Anna Nowak', email: 'anna@example.com', role: 'Editor' },
    {
      id: 3,
      name: 'Piotr Zieliński',
      email: 'piotr@example.com',
      role: 'User',
    },
    {
      id: 4,
      name: 'Piotr Zieliński',
      email: 'piotr@example.com',
      role: 'User',
    },
    {
      id: 5,
      name: 'Piotr Zieliński',
      email: 'piotr@example.com',
      role: 'User',
    },
    {
      id: 6,
      name: 'Piotr Zieliński',
      email: 'piotr@example.com',
      role: 'User',
    },
  ]);

  readonly columns: ColumnDef<User>[] = [
    {
      accessorKey: 'id',
      header: 'ID',
    },
    {
      accessorKey: 'name',
      header: 'Imię i Nazwisko',
    },
    {
      accessorKey: 'email',
      header: 'Email',
    },
    {
      accessorKey: 'role',
      header: 'Rola',
    },
    {
      accessorKey: 'actions',
      header: '',
    },
  ];

  readonly table = createAngularTable(() => ({
    data: this.data(),
    columns: this.columns,
    getCoreRowModel: getCoreRowModel(),
  }));
}
