import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'app-gm',
  templateUrl: './gm.component.html',
  styleUrls: ['./gm.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class GmComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}
