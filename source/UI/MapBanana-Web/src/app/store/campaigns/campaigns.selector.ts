import { IAppState } from 'src/app/app.state';
import { ICampaign } from './campaigns.model';

import { createSelector } from '@ngrx/store';
 
export const selectCampaigns = createSelector(
    (state: IAppState) => state.campaigns,
    (campaigns: Array<ICampaign>) => campaigns
);
 
// export const selectCampaign = (id: string) => createSelector(
//     (state: IAppState) => state.campaigns,
//     (campaigns: Map<string, ICampaign>) => campaigns.get(id)
// );