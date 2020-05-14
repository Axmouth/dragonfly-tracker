import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-message-area',
  templateUrl: './message-area.component.html',
  styleUrls: ['./message-area.component.scss'],
})
export class MessageAreaComponent implements OnInit {
  @Input()
  errors: string[] = [];
  @Input()
  messages: string[];
  @Input()
  loading = false;

  constructor() {}

  ngOnInit() {}
}
