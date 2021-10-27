import { HttpClient } from '@angular/common/http';
import * as signalR from '@microsoft/signalr';
import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'app-gm',
  templateUrl: './gm.component.html',
  styleUrls: ['./gm.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class GmComponent implements OnInit {

  constructor(
    private httpClient: HttpClient,) {

    httpClient.get('https://localhost:5001/api/campaign/campaigns').subscribe(response => {
      console.log(response);
    });
  }

  ngOnInit(): void {
  }
}
